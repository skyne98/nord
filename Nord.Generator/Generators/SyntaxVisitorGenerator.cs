using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Nord.Generator.Models;

namespace Nord.Generator.Generators
{
    public class SyntaxVisitorGenerator
    {
        public static void GenerateSyntaxVisitors(List<SyntaxNodeModel> models)
        {
            // Clear the folder
            var directories = Directory.GetDirectories(Config.VisitorDirectory).ToList();
            var files = Directory.GetFiles(Config.VisitorDirectory).ToList();
            directories.ForEach(directory => files.AddRange(Directory.GetFiles(directory)));
            files.ForEach(File.Delete);
            directories.ForEach(Directory.Delete);
            
            // Generate files
            var workspace = new AdhocWorkspace();
            var generator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);
            var generatedUnits = new List<CompilationUnitSyntax>();
            GenerateSyntaxVisitor(models, generator);
        }

        private static void GenerateSyntaxVisitor(List<SyntaxNodeModel> models, SyntaxGenerator generator)
        {
            var className = "SyntaxVisitor";
            
            // Generate basic usings
            var usingSystem = generator.NamespaceImportDeclaration("System");
            var usingCollections = generator.NamespaceImportDeclaration("System.Collections.Generic");
            var usingIo = generator.NamespaceImportDeclaration("System.IO");
            var usingBinary = generator.NamespaceImportDeclaration("System.Runtime.Serialization.Formatters.Binary");
            var usingLanguageExt = generator.NamespaceImportDeclaration("LanguageExt");
            var usingAst = generator.NamespaceImportDeclaration("Nord.Compiler.Ast");
            var usedModels = new List<SyntaxNodeModel>();
            
            // Generate class
            var classNode = (ClassDeclarationSyntax)CSharpSyntaxTree.ParseText($@"
                public abstract class {className} {{
                    
                }}
            ").GetRoot().ChildNodes().First();
            
            // Generate default visit
            var defaultVisitNode = (MemberDeclarationSyntax)CSharpSyntaxTree.ParseText($@"
                public virtual void DefaultVisit(SyntaxNode node) {{

                }}
            ").GetRoot().ChildNodes().First();
            classNode = classNode.AddMembers(defaultVisitNode);
            
            // Generate visit
            var visitNode = (MemberDeclarationSyntax)CSharpSyntaxTree.ParseText($@"
                public virtual void Visit(SyntaxNode node) {{
                    if (node == null)
                    {{
                        return;
                    }}
         
                    node.Accept(this);
                }}
            ").GetRoot().ChildNodes().First();
            classNode = classNode.AddMembers(visitNode);

            // Generate specific visit methods
            foreach (var nodeModel in models)
            {
                var nodeClassName = nodeModel.GetClassName();
            }
        }
    }
}