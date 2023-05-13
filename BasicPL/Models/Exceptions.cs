using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BasicPL.Models
{
    public class BasicException : Exception
    {
        public Position StartPosition;
        public Position EndPosition;

        public BasicException(string? message, Position startPos, Position endPos)
            : base(ExceptionMessage(message, startPos, endPos))
        {
            StartPosition = startPos;
            EndPosition = endPos;
        }

        private static string ExceptionMessage(string? message, Position startPos, Position endPos) => $"Exception: {message}\nFile: {startPos.FileName}, Line: {startPos.LineNum + 1}";
    }

    public class IllegalCharException : BasicException
    {
        public IllegalCharException(char illegalChar, Position startPos, Position endPos)
            : base($"Illegal Character: '{illegalChar}'", startPos, endPos)
        {
        }
    }

    public class InvalidSyntaxException : BasicException
    {
        public InvalidSyntaxException(string? message, Position startPos, Position endPos)
            : base($"Invalid Syntax: '{message}'", startPos, endPos)
        {
        }
    }

    public class ExpectedCharException : BasicException
    {
        public ExpectedCharException(string? message, Position startPos, Position endPos)
            : base($"Expected Character: {message}", startPos, endPos)
        {
        }
    }


    public class RuntimeException : BasicException
    {
        public RuntimeException(string? message, Position startPos, Position endPos, Context ctx)
            : base($"Runtime Exception: '{ExceptionMessage(message, startPos, ctx)}'", startPos, endPos)
        {
        }

        private static string ExceptionMessage(string? message, Position startPos, Context ctx)
        {
            return GenerateTraceback(startPos, ctx) + "\n" + message;
        }

        private static string GenerateTraceback(Position startPos, Context ctx)
        {
            string result = "";
            while (ctx != null)
            {
                result = $"\tFile {startPos.FileName}, Line {startPos.LineNum + 1}, in {ctx.DisplayName}\n" + result;
                startPos = ctx.ParentEntryPosition;
                ctx = ctx.Parent;
            }

            return "Traceback (most recent call last):\n" + result;
        }
    }
}
