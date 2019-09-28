using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using LanguageExt;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using Nord.Generator.Models;
using Humanizer;
using LanguageExt.UnsafeValueAccess;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Nord.Generator.Generators
{
    public class SyntaxNodesGenerator
    {
        public readonly static string AstClassesDirectory = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../../Nord.Compiler/Generated/Ast/"));

        public static void GenerateSyntaxNodes(List<SyntaxNodeModel> models)
        {
            // Clear the folder
            var directories = Directory.GetDirectories(AstClassesDirectory).ToList();
            var files = Directory.GetFiles(AstClassesDirectory).Where(f => !f.Contains("AstDefinitions")).ToList();
            directories.ForEach(directory => files.AddRange(Directory.GetFiles(directory)));
            files.ForEach(File.Delete);
            directories.ForEach(Directory.Delete);
            
            // Generate files
            var workspace = new AdhocWorkspace();
            var generator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);
            var generatedUnits = new List<CompilationUnitSyntax>();

            models.ForEach(m => GenerateClass(m, models, generatedUnits, generator));
        }

        private static void GenerateClass(SyntaxNodeModel model, List<SyntaxNodeModel> models,
            List<CompilationUnitSyntax> compilationUnits,
            SyntaxGenerator generator)
        {
            // Make names
            var className = model.GetClassName();
            var parent = model.GetParent(models);
            var parentName = parent.Map(p => p.GetName());
            var parentClassName = parent.Map(p => p.GetClassName());
            var root = model.GetRootParent(models);
            var rootName = root.Map(r => r.GetName());
            var rootClassName = root.Map(r => r.GetClassName());

            // Generate basic usings
            var usingSystem = generator.NamespaceImportDeclaration("System");
            var usingCollections = generator.NamespaceImportDeclaration("System.Collections.Generic");
            var usingIo = generator.NamespaceImportDeclaration("System.IO");
            var usingBinary = generator.NamespaceImportDeclaration("System.Runtime.Serialization.Formatters.Binary");
            var usingLanguageExt = generator.NamespaceImportDeclaration("LanguageExt");
            var usingAst = generator.NamespaceImportDeclaration("Nord.Compiler.Ast");
            var usedModels = new List<SyntaxNodeModel>();
            parent.IfSome(p => usedModels.Add(p));

            // Generate class
            var classNode = (ClassDeclarationSyntax) parentClassName.Match(pn => generator.ClassDeclaration(className,
                    accessibility: Accessibility.Public,
                    baseType: generator.IdentifierName(pn)),
                () => generator.ClassDeclaration(className,
                    accessibility: Accessibility.Public));
            classNode = classNode.AddAttributeLists(SyntaxFactory.AttributeList()
                .AddAttributes(
                    SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Serializable"))));

            // Generate properties
            foreach (var parameter in model.Parameters)
            {
                var declaration = SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.ParseTypeName(parameter.Value), parameter.Key.Pascalize())
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddAccessorListAccessors(
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)));
                classNode = classNode.AddMembers(declaration);
            }

            // Generate copy method
            if (parentName.IsNone)
            {
                var copyDeclarationText = @"
                    public T Copy<T>() where T: SyntaxNode
                    {
                         using (var ms = new MemoryStream())
                         {
                               var formatter = new BinaryFormatter();
                               formatter.Serialize(ms, this);
                               ms.Position = 0;

                               return (T)formatter.Deserialize(ms);
                         }
                    }
                ";
                var copyDeclarationParsed = CSharpSyntaxTree.ParseText(copyDeclarationText);
                var copyDeclaration = (MemberDeclarationSyntax)copyDeclarationParsed.GetRoot().ChildNodes().First();
                classNode = classNode.AddMembers(copyDeclaration);   
            }

            // Generate with methods
            foreach (var parameter in model.Parameters)
            {
                var methodDeclaration = (MethodDeclarationSyntax)generator.MethodDeclaration(
                    $"With{parameter.Key.Pascalize()}");
                methodDeclaration = methodDeclaration.WithReturnType(
                    (TypeSyntax) generator.IdentifierName(className));
                methodDeclaration = methodDeclaration.AddParameterListParameters(
                    (ParameterSyntax) generator.ParameterDeclaration(
                        "value",
                        SyntaxFactory.ParseTypeName(parameter.Value)
                    )
                    );
                methodDeclaration = methodDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                var body = SyntaxFactory.Block();
                body = body.AddStatements(
                    (StatementSyntax) generator.LocalDeclarationStatement(
                        "copy",
                        generator.CastExpression(
                            generator.IdentifierName(model.GetClassName()),
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.AsExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        (ExpressionSyntax) generator.BaseExpression(),
                                        SyntaxFactory.GenericName("Copy")
                                            .AddTypeArgumentListArguments(SyntaxFactory.IdentifierName(className))
                                    )
                                ),
                                SyntaxFactory.IdentifierName(className)
                            )
                        )
                    )
                );
                body = body.AddStatements(
                    (StatementSyntax) generator.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                (ExpressionSyntax) generator.IdentifierName("copy"),
                                (SimpleNameSyntax) generator.IdentifierName(parameter.Key.Pascalize())
                            ),
                            (ExpressionSyntax) generator.IdentifierName("value")
                        )
                    )
                );
                body = body.AddStatements(
                    (StatementSyntax) generator.ReturnStatement(
                        generator.IdentifierName("copy")    
                    )
                );
                methodDeclaration = methodDeclaration.WithBody(body);
                
                classNode = classNode.AddMembers(methodDeclaration);
            }
            
            // Find nodes in parameters
            model.Parameters.ToList().ForEach(p =>
            {
                var possibleParameterModels = models
                    .Where(pm => p.Value.Contains(pm.GetClassName()))
                    .Where(pm => pm.GetClassName() != className)
                    .Where(pm => pm.GetNamespace(models) != model.GetNamespace(models));
                
                usedModels.AddRange(possibleParameterModels);
            });
            
            // Clean up the used nodes
            usedModels = usedModels.Distinct().ToList();
                
            // Generate namespace
            var namespaceNode = (NamespaceDeclarationSyntax)generator.NamespaceDeclaration(model.GetNamespace(models));
            namespaceNode = namespaceNode.AddMembers(classNode);
            
            // Create compilation unit
            var compilationUnit = (CompilationUnitSyntax)generator.CompilationUnit();
            compilationUnit = compilationUnit.AddUsings((UsingDirectiveSyntax) usingSystem);
            compilationUnit = compilationUnit.AddUsings((UsingDirectiveSyntax) usingCollections);
            compilationUnit = compilationUnit.AddUsings((UsingDirectiveSyntax) usingIo);
            compilationUnit = compilationUnit.AddUsings((UsingDirectiveSyntax) usingBinary);
            compilationUnit = compilationUnit.AddUsings((UsingDirectiveSyntax) usingLanguageExt);
            compilationUnit = compilationUnit.AddUsings((UsingDirectiveSyntax) usingAst);
            foreach (var usedNamespace in usedModels.Select(um => um.GetNamespace(models)))
            {
                compilationUnit = compilationUnit.AddUsings(
                    (UsingDirectiveSyntax) generator.NamespaceImportDeclaration(usedNamespace));
            }
            compilationUnit = compilationUnit.AddMembers((MemberDeclarationSyntax) namespaceNode);
            
            // Format
            compilationUnit = compilationUnit.NormalizeWhitespace();
            
            // Add to the lists
            compilationUnits.Add(compilationUnit);
            
            // Write to file
            var fileDirectory = parentName.IsSome
                ? Path.Combine(AstClassesDirectory, parentName.ValueUnsafe().Pascalize().Pluralize())
                : AstClassesDirectory;
            var filePath = Path.Combine(fileDirectory, className);
            
            // Log to console
            Console.WriteLine($"Generating {classNode.Identifier.Text}");
            Console.WriteLine($"\tParent {parentClassName.IfNone("None")}");
            foreach (var usedModel in usedModels)
            {
                Console.WriteLine($"\tUsed {usedModel.Name} from {usedModel.GetNamespace(models)}");
            }

            if (Directory.Exists(fileDirectory) == false)
                Directory.CreateDirectory(fileDirectory);
            File.WriteAllText(Path.ChangeExtension(filePath, ".cs"), compilationUnit.GetText().ToString());
        }
    }
}