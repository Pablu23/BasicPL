using BasicPL.Models;
using BasicPL.Nodes;
using System;
using System.Collections.Specialized;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace BasicPL.Core
{
    public class Interpreter
    {
        public Number Visit(BaseNode node, Context context)
        {
            return node switch
            {
                BinOpNode => VisitBinOpNode((BinOpNode)node, context),
                NumberNode => VisitNumberNode((NumberNode)node, context),
                UnaryOpNode => VisitUnaryNode((UnaryOpNode)node, context),
                VarAccessNode => VisitVarAccessNode((VarAccessNode)node, context),
                VarAssignNode => VisitVarAssignNode((VarAssignNode)node, context),
                _ => throw new NotImplementedException(),
            };
        }

        private Number VisitVarAccessNode(VarAccessNode node, Context context)
        {
            string varName = (string)node.VarNameToken.Value!;
            dynamic? value = context.SymbolTable.Get(varName);

            if (value == null)
                throw new RuntimeException($"'{varName}' is not defined", node.Start, node.End, context);

            return new Number(value);
        }

        private Number VisitVarAssignNode(VarAssignNode node, Context ctx)
        {
            string varName = (string)node.VarNameToken.Value!;
            var value = Visit(node.ValueNode, ctx);
            var assigned = ctx.SymbolTable.Set(varName, value, node.IsReadonly);
            if (!assigned)
                throw new RuntimeException($"'{varName}' is constant and cannot be redefined", node.Start, node.End, ctx);
            return value;
        }

        private Number VisitNumberNode(NumberNode node, Context ctx)
        {
            if (node.Token.Type.TryGetType(out Type? type))
            {
                var val = Convert.ChangeType(node.Token.Value, type);
                if (val == null)
                    throw new NotImplementedException();

                return new Number(val).SetPos(node.Start, node.End).SetContext(ctx);
            }

            throw new NotImplementedException();
        }

        private Number VisitBinOpNode(BinOpNode node, Context ctx)
        {
            var left = Visit(node.Left, ctx);
            var right = Visit(node.Right, ctx);


            var num = (node.Token.Type, (string?)node.Token.Value) switch
            {
                (TokenType.PLUS, null) => left.AddBy(right.Value),
                (TokenType.MINUS, null) => left.SubBy(right.Value),
                (TokenType.MUL, null) => left.MulBy(right.Value),
                (TokenType.DIV, null) => left.DivBy(right.Value),
                (TokenType.POW, null) => left.PowBy(right.Value),
                (TokenType.EQ, null) => left.GetCompEQ(right.Value),
                (TokenType.NE, null) => left.GetCompNE(right.Value),
                (TokenType.LT, null) => left.GetCompLT(right.Value),
                (TokenType.GT, null) => left.GetCompGT(right.Value),
                (TokenType.LTE, null) => left.GetCompLTE(right.Value),
                (TokenType.GTE, null) => left.GetCompGTE(right.Value),
                (TokenType.KEYWORD, Keywords.AND) => left.AndBy(right.Value),
                (TokenType.KEYWORD, Keywords.OR) => left.OrBy(right.Value),

                _ => throw new NotImplementedException(),
            };

            return num.SetPos(node.Start, node.End);
        }

        private Number VisitUnaryNode(UnaryOpNode node, Context ctx)
        {
            var num = Visit(node.Node, ctx);

            switch ((node.Token.Type, (string?)node.Token.Value))
            {
                case (TokenType.MINUS, null):
                    num = num.MulBy(-1);
                    break;

                case (TokenType.KEYWORD, Keywords.NOT):
                    num = num.Not();
                    break;

                default:
                    throw new NotImplementedException();
            }
            return num.SetPos(node.Start, node.End);
        }
    }
}
