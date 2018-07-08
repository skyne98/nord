using System;
using Newtonsoft.Json;
using Nord.Compiler.Lexer;
using Nord.Compiler.Parser;
using Superpower;
using Superpower.Model;

namespace Nord.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = "'Hello world'";
            object value;
            string error;
            Position errorPosition;
            var result = Parsers.TryParse(code, out value, out error, out errorPosition);

            if (String.IsNullOrEmpty(error))
            {
                Console.WriteLine(JsonConvert.SerializeObject(value));
            }
            else
            {
                Console.WriteLine("{0}", error);
            }

            Console.ReadLine();
        }
    }
}