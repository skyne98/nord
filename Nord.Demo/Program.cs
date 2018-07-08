using System;
using Newtonsoft.Json;
using Nord.Compiler.Lexer;
using Nord.Compiler.Parser;
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
                        Console.WriteLine(serializer.Serialize(value));
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