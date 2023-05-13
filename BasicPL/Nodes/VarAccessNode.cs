using BasicPL.Models;

namespace BasicPL.Nodes
{
    public class VarAccessNode : BaseNode
    {
        public Token VarNameToken;

        public VarAccessNode(Token token) 
            : base(token)
        {
            VarNameToken = token;
        }
    }
}