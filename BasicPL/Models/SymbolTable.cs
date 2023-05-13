using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicPL.Models
{
    public class SymbolTable
    {
        public Dictionary<string, (dynamic, bool locked)> Symbols;
        public SymbolTable? Parent;

        public SymbolTable()
        {
            Symbols = new Dictionary<string, (dynamic, bool)>();
            Parent = null;
        }

        public dynamic? Get(string name)
        {
            if (Symbols.TryGetValue(name, out var value))
                return value.Item1;
            if (Parent != null)
                return Parent.Get(name);
            return null;
        }

        public bool Set(string name, dynamic value, bool isReadonly = false)
        {
            if (Symbols.ContainsKey(name) && Symbols[name].locked) return false;

            Symbols[name] = (value, isReadonly);
            return true;
        }

        public void Remove(string name)
        {
            Symbols.Remove(name);
        }
    }
}
