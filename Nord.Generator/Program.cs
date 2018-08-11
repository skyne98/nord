using System;
using Nord.Generator.Generators;

namespace Nord.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var models = SyntaxNodesDeserializer.GetSyntaxNodes();
            SyntaxNodesGenerator.GenerateSyntaxNodes(models);
        }
    }
}