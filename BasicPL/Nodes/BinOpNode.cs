using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicPL.Models;

namespace BasicPL.Nodes
{
    public class BinOpNode : BaseNode
    {
        public BaseNode Left, Right;
        public BinOpNode(Token token, BaseNode left, BaseNode right)
            : base(token)
        {
            Left = left;
            Right = right;

            Start = left.Start;
            End = right.End;
        }

        public override string ToString()
        {
            return $"({Left}, {Token}, {Right})";
        }
    }
}
