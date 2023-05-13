using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicPL.Models;

namespace BasicPL.Nodes
{
    public class BaseNode
    {
        public Token Token;
        public Position Start;
        public Position End;

        public BaseNode(Token token)
        {
            Token = token;

            Start = token.Start;
            End = token.End;
        }

        public override string ToString()
        {
            return $"{Token}";
        }
    }
}
