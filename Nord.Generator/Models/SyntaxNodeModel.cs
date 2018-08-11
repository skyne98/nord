using System.Collections.Generic;
using LanguageExt;

namespace Nord.Generator.Models
{
    public class SyntaxNodeModel
    {
        public SyntaxNodeModel(string name, Option<string> parent, Dictionary<string, string> parameters)
        {
            Name = name;
            Parent = parent;
            Parameters = parameters;
        }

        public string Name { get; set; }
        public Option<string> Parent { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}