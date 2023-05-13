using BasicPL.Models;

namespace BasicPL.Nodes
{
    public class VarAssignNode : BaseNode
    {
        public Token VarNameToken;
        public BaseNode ValueNode;
        public bool IsReadonly;

        public VarAssignNode(Token varName, BaseNode valueNode, bool isReadonly = false)
            : base(varName)
        {
            this.VarNameToken = varName;
            this.ValueNode = valueNode;
            IsReadonly = isReadonly;
        }
    }
}