using BasicPL.Core;
using BasicPL.Models;
using BasicPL.Nodes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;

namespace BasicPL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            SymbolTable globalSymboleTable = new SymbolTable();
            globalSymboleTable.Set("null", new Number(0), true);
            globalSymboleTable.Set("true", new Number(1), true);
            globalSymboleTable.Set("false", new Number(0), true);
            while (true)
            {
                Console.Write("Basic > ");
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Empty String not supported");
                    return;
                }
                if (input == "pr_var")
                {
                    Console.WriteLine("Printing Variables: ");
                    foreach (var variable in globalSymboleTable.Symbols)
                    {
                        Console.WriteLine($"\t{variable.Key} \t= {variable.Value.Item1}:{variable.Value.Item1.Value.GetType()} \t| Locked: {variable.Value.locked}");
                    }
                }
                else
                {
                    var result = Run("<stdin>", input, globalSymboleTable);
                    if (result is not null)
                        Console.WriteLine(result);
                }



                //Console.WriteLine(ast);

                //var tokens = Run("<stdin>", input);

                //if (tokens is not null)
                //{
                //    Console.Write('[');
                //    foreach (var token in tokens)
                //    {
                //        Console.Write(token);

                //        if (tokens.IndexOf(token) != tokens.Count - 1)
                //            Console.Write(", ");
                //    }
                //    Console.WriteLine(']');
                //}
            }
        }

        private static Number? Run(string fileName, string text, SymbolTable global)
        {
            try
            {
                Lexer lex = new Lexer(fileName, text);
                var tokens = lex.MakeTokens();
                Parser parser = new Parser(tokens);
                var ast = parser.Parse();
                Interpreter interpreter = new Interpreter();
                Context ctx = new Context("<program>");
                ctx.SymbolTable = global;
                return interpreter.Visit(ast, ctx);
            }
            catch (BasicException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}