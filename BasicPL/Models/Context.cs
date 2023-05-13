using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicPL.Models
{
    public class Context /*: IEnumerable<Context>*/
    {
        public string DisplayName;
        public Context? Parent;
        public Position ParentEntryPosition;
        public SymbolTable SymbolTable;

        public Context(string displayName)
        {
            DisplayName = displayName;
            SymbolTable = new SymbolTable();
        }

        public Context(string displayName, Context parent, Position parentEntryPosition)
        {
            DisplayName = displayName;
            Parent = parent;
            ParentEntryPosition = parentEntryPosition;
            SymbolTable = new SymbolTable();
            SymbolTable.Parent = Parent.SymbolTable;
        }

        //public IEnumerator GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //IEnumerator<Context> IEnumerable<Context>.GetEnumerator()
        //{
        //    yield return Parent;
        //}
    }
}
