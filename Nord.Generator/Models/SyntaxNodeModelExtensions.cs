using System.Collections.Generic;
using System.Linq;
using Humanizer;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Nord.Generator.Models
{
    public static class SyntaxNodeModelExtensions
    {
        public static Option<SyntaxNodeModel> GetParent(this SyntaxNodeModel model, List<SyntaxNodeModel> models)
        {
            return model.Parent.Map(p => models.FirstOrDefault(m => m.Name == p));
        }
        
        public static Option<SyntaxNodeModel> GetRootParent(this SyntaxNodeModel model, List<SyntaxNodeModel> models)
        {
            var parent = model.GetParent(models);
            var root = parent.Map(p => p.GetRootParent(models).ValueUnsafe());

            return root.Match(
                r => r,
                () => parent
            );
        }
        
        public static string GetNamespace(this SyntaxNodeModel model, List<SyntaxNodeModel> models)
        {
            return model.GetParent(models).Match(
                parent => $"Nord.Compiler.Generated.Ast.{parent.GetName().Pluralize()}",
                () => "Nord.Compiler.Generated.Ast"
            );
        }

        public static string GetName(this SyntaxNodeModel model)
        {
            return model.Name;
        }

        public static string GetClassName(this SyntaxNodeModel model)
        {
            return $"Syntax{model.Name.Pascalize()}";
        }

        public static SyntaxNodeModel GetBase(this SyntaxNodeModel model, List<SyntaxNodeModel> models)
        {
            var rootOption = model.GetRootParent(models);

            return rootOption.Match(
                root => root,
                () => model
                );
        }
    }
}