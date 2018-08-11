using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageExt;
using Newtonsoft.Json.Linq;
using Nord.Generator.Models;

namespace Nord.Generator.Generators
{
    public class SyntaxNodesDeserializer
    {
        public readonly static string AstDefinitionsFile = Path.GetFullPath("../Nord.Compiler/Generated/Ast/AstDefinitions.json");

        public static List<SyntaxNodeModel> GetSyntaxNodes()
        {
            var defintionsContents = File.ReadAllText(AstDefinitionsFile);
            var defintionsJson = JObject.Parse(defintionsContents);
            var definitionsObject = defintionsJson.GetValue("definitions") as JObject;
            return definitionsObject.Properties()
                .Select(prop =>
                {
                    var name = prop.Name;
                    var def = prop.Value as JObject;
                    if (def.Properties().Any())
                    {
                        var parent = def.Properties().Any(defProp => defProp.Name == "parent")
                            ? def.Value<string>("parent")
                            : Option<string>.None;
                        var properties = def.Properties().Any(defProp => defProp.Name == "parameters")
                            ? new Dictionary<string, string>(
                                def.Value<JObject>("parameters")
                                    .Properties().Select(propProp =>
                                        new KeyValuePair<string, string>(propProp.Name, 
                                            propProp.Value.Value<string>())))
                            : new Dictionary<string, string>();
                        return new SyntaxNodeModel(name, parent, properties);
                    }

                    return new SyntaxNodeModel(name, null, new Dictionary<string, string>());
                }).ToList();
        }
    }
}