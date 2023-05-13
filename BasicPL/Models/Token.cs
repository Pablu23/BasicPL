using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BasicPL.Models
{
    [Flags]
    public enum TokenType // : ulong
    {
        NONE = 1 << 0,
        INT = 1 << 1,
        FLOAT = 1 << 2,
        PLUS = 1 << 3,
        MINUS = 1 << 4,
        MUL = 1 << 5,
        DIV = 1 << 6,
        LPAREN = 1 << 7,
        RPAREN = 1 << 8,
        EOF = 1 << 9,
        POW = 1 << 10,
        IDENTIFIER = 1 << 11,
        KEYWORD = 1 << 12,
        ASSIGN = 1 << 13,
        EQ = 1 << 14,
        NE = 1 << 15,
        GT = 1 << 16,
        LT = 1 << 17,
        LTE = 1 << 18,
        GTE = 1 << 19
    }

    public static class Keywords
    {
        public const string VAR = "var";
        public const string AND = "and";
        public const string OR = "or";
        public const string NOT = "not";
        public const string LET = "let";

        private static readonly string[] _constants = typeof(Keywords).GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
            .Select(x => x.GetRawConstantValue())
            .Where(x => x != null && x.GetType() == typeof(string))
            .Select(x => (string)x!)
            .ToArray();

        public static bool IsKeyword(string iden)
        {
            return _constants.Any(x => x == iden/*x.Equals(iden, StringComparison.OrdinalIgnoreCase)*/);
        }
    }

    public static class TokenExtensions
    {
        public static bool TryGetType(this TokenType tokenType, [NotNullWhen(true)] out Type? type)
        {
            switch (tokenType)
            {
                case TokenType.INT:
                    type = typeof(int);
                    return true;
                case TokenType.FLOAT:
                    type = typeof(float);
                    return true;
                default:
                    type = null;
                    return false;
            };
        }
    }

    public class Token
    {
        public TokenType Type;
        public object? Value;
        public Position Start;
        public Position End;

        public Token(TokenType type, Position start, object? value = null, Position? end = null)
        {
            Type = type;
            Value = value;
            Start = start;
            start.Advance(' ');
            End = end ?? start;
        }

        public override string ToString()
        {
            return Value is null ? $"[{Type}]" : $"[{Type}: {Value}]";
        }
    }
}
