using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using Nord.Compiler;
using Nord.Compiler.Ast;
using Nord.Compiler.Generated.Ast;
using Nord.Compiler.Lexer;
using Nord.Compiler.Parser;
using Nord.Compiler.Pass;
using Superpower;
using Superpower.Model;
using YamlDotNet.Serialization;

namespace Nord.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("nord> ");
            var line = Console.ReadLine();
            while (line != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (Parsers.TryParse(line, out var value, out var error, out var errorPosition))
                    {
                        var serializer = new SerializerBuilder().Build();
                        try
                        {
                            Console.WriteLine("AST:");
                            Console.WriteLine(serializer.Serialize(value));
                            var context = new Context(value as SyntaxNode);
                            var transformed = context.Require<HirTransformerPass>();
                            Console.WriteLine("HIR:");
                            Console.WriteLine(serializer.Serialize(transformed));
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Console.WriteLine("AST:");
                                Console.WriteLine(JsonConvert.SerializeObject(value, Formatting.Indented));
                                var context = new Context(value as SyntaxNode);
                                var transformed = context.Require<HirTransformerPass>();
                                Console.WriteLine("HIR:");
                                Console.WriteLine(JsonConvert.SerializeObject(transformed, Formatting.Indented));
                            }
                            catch (Exception nestedEx)
                            {
                                Console.WriteLine("There was a problem with serialization");
                                throw nestedEx;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"     {new string(' ', errorPosition.Column)}^");
                        Console.WriteLine(error);
                    }
                }
                
                Console.Write("nord> ");
                line = Console.ReadLine();
            }
        }
    }
}