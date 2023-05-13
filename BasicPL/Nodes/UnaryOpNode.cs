using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicPL.Models;

namespace BasicPL.Nodes
{
    public class UnaryOpNode : BaseNode
    {
        public BaseNode Node;
        public UnaryOpNode(Token token, BaseNode node)
            : base(token)
        {
            Node = node;

            Start = token.Start;
            End = node.End;
        }

        public override string ToString()
        {
            return $"({Token}, {Node})";
        }
    }
}
