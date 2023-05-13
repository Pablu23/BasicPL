using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using BasicPL.Models;
using BasicPL.Nodes;

namespace BasicPL.Core
{
    public class Parser
    {
        private List<Token> _tokens;
        private int _index;
        private Token _currToken;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _index = -1;
            _currToken = new Token(TokenType.NONE, new Position(0, 0, 0, "null", "null"));
            Advance();
        }

        private void Advance()
        {
            _index++;
            if (_index < _tokens.Count)
                _currToken = _tokens[_index];

            //return _currToken;
        }

        public BaseNode Parse()
        {
            var res = Expression();
            if (_currToken.Type != TokenType.EOF)
                throw new InvalidSyntaxException("Expected '+', '-', '*' or '/'", _currToken.Start, _currToken.End);
            return res;
        }

        private BaseNode Atom()
        {
            var token = _currToken;

            switch (token.Type)
            {
                case var type when (TokenType.INT | TokenType.FLOAT).HasFlag(type):
                    Advance();
                    return new NumberNode(token);

                case TokenType.IDENTIFIER:
                    Advance();
                    return new VarAccessNode(token);

                case TokenType.LPAREN:
                    Advance();
                    var expr = Expression();
                    if (_currToken.Type == TokenType.RPAREN)
                    {
                        Advance();
                        return expr;
                    }
                    throw new InvalidSyntaxException("Expected ')'", _currToken.Start, _currToken.End);
                default:
                    throw new InvalidSyntaxException("Expected int, float, +, -, (, Identifier or Keyword", token.Start, token.End);

            }
        }

        private BaseNode Factor()
        {
            var token = _currToken;

            if ((TokenType.PLUS | TokenType.MINUS).HasFlag(token.Type))
            {
                Advance();
                var fact = Factor();
                return new UnaryOpNode(token, fact);
            }

            return Power();
            //else throw new InvalidSyntaxException("Expected int or float", token.Start, token.End);
        }

        private BaseNode Power()
        {
            return BinOp(Atom, TokenType.POW, Factor);
        }

        private BaseNode Term()
        {
            return BinOp(Factor, TokenType.MUL | TokenType.DIV | TokenType.POW);
        }

        private BaseNode Expression()
        {
            if (_currToken.Type == TokenType.KEYWORD && _currToken.Value is string s && (s == Keywords.VAR || s == Keywords.LET))
            {
                Advance();

                if (_currToken.Type != TokenType.IDENTIFIER)
                    throw new InvalidSyntaxException("Expected Identifier", _currToken.Start, _currToken.End);

                var varName = _currToken;

                Advance();

                if (_currToken.Type != TokenType.ASSIGN)
                    throw new InvalidSyntaxException("Expected '='", _currToken.Start, _currToken.End);

                Advance();
                var expr = Expression();
                return new VarAssignNode(varName, expr, s == Keywords.LET);
            }
            //else if(_currToken.Type == TokenType.KEYWORD && _currToken.Value is string s2 && s2 == Keywords.LET)
            //{
            //    Advance();

            //    if(_currToken.Type != TokenType.IDENTIFIER)
            //        throw new InvalidSyntaxException("Expected Identifier", _currToken.Start, _currToken.End);

            //    var varName = _currToken;

            //    Advance();

            //    if(_currToken.Type != TokenType.ASSIGN)
            //        throw new InvalidSyntaxException("Expected '='", _currToken.Start, _currToken.End);

            //    Advance();
            //    var expr = Expression();
            //    return new VarAssignNode(varName, expr, true);
            //}

            return BinOp(CompExpr, TokenType.KEYWORD, null, Keywords.AND, Keywords.OR);
        }

        private BaseNode CompExpr()
        {
            BaseNode node;
            if (_currToken.Type == TokenType.KEYWORD && _currToken.Value is string s && s == Keywords.NOT)
            {
                var token = _currToken;
                Advance();

                node = CompExpr();
                return new UnaryOpNode(token, node);
            }

            node = BinOp(ArithExpr, TokenType.EQ | TokenType.NE | TokenType.LT | TokenType.GT | TokenType.LTE | TokenType.GTE);
            return node;
        }

        private BaseNode ArithExpr()
        {
            return BinOp(Term, TokenType.PLUS | TokenType.MINUS);
        }

        private BaseNode BinOp(Func<BaseNode> lFunc, TokenType ops, Func<BaseNode>? rFunc = null, params string[] keywords)
        {
            var left = lFunc();
            rFunc ??= lFunc;

            while (_currToken is not null && (ops.HasFlag(_currToken.Type) ||
                _currToken.Type == TokenType.KEYWORD && !string.IsNullOrEmpty((string?)_currToken.Value) && keywords.Contains((string)_currToken.Value)))
            {
                var opToken = _currToken;
                Advance();
                var right = rFunc();
                left = new BinOpNode(opToken, left, right);
            }

            return left;
        }
    }
}
