using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using LanguageExt.UnsafeValueAccess;
using Nord.Compiler.Ast;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using LanguageExt;
using Nord.Compiler.Generated.Ast;
using Nord.Compiler.Generated.Ast.DestructuringPatterns;
using Nord.Compiler.Generated.Ast.ExpressionLiterals;
using Nord.Compiler.Generated.Ast.ExpressionPostfixes;
using Nord.Compiler.Generated.Ast.Expressions;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.StatementLets;
using Nord.Compiler.Generated.Ast.Statements;

namespace Nord.Compiler.Pass
{
    public class HirTransformerPass : ICompilerPass
    {
        public SyntaxNode Run(Context context)
        {
            var root = DeepClone(context.Ast);
            return Transform(context, root).FirstOrDefault();
        }

        private SyntaxNode[] Transform(Context context, SyntaxNode node)
        {
            switch (node)
            {
                case SyntaxDocument documentNode:
                    return new[] {new SyntaxDocument().WithStatements(documentNode.Statements
                        .SelectMany(st => Transform(context, st)).Cast<SyntaxStatementTopLevel>().ToArray())};
                case SyntaxStatement statementNode:
                    return TransformStatement(context, statementNode);
            }
            
            return new SyntaxNode[] { DeepClone(node) };
        } 
        
        private SyntaxNode[] TransformStatement(Context context, SyntaxStatement node)
        {
            switch (node)
            {
                case SyntaxStatementLetDestructuring letDestructuring:
                {
                    return TransformDestructuringLetStatement(context, letDestructuring.Pattern, letDestructuring.Expression)
                        .Cast<SyntaxNode>().ToArray();
                    break;
                }
                case SyntaxStatementTopLevel topLevelNode:
                {
                    return new SyntaxNode[]
                    {
                        new SyntaxStatementTopLevel()
                            .WithModifiers(topLevelNode.Modifiers)
                            .WithStatement(
                            Transform(context, topLevelNode.Statement)
                                .FirstOrDefault() as SyntaxStatement)
                    };
                }
                case SyntaxStatementFunction functionNode:
                {
                    return new[] { new SyntaxStatementFunction().WithName(functionNode.Name).WithTypeParameters(functionNode.TypeParameters).WithParameters(functionNode.Parameters).WithReturn(functionNode.Return).WithBody(functionNode.Body.SelectMany(bs => Transform(context, bs))
                            .Cast<SyntaxStatement>()
                            .ToArray())};
                }
            }

            return new SyntaxStatement[] { DeepClone(node) };
        }

        private SyntaxStatementLet[] TransformDestructuringLetStatement(Context context, SyntaxDestructuringPattern pattern,
            SyntaxExpression value)
        {
            var letName = context.GenerateVariableName();
            var letValue = Transform(context, value).FirstOrDefault() as SyntaxExpression;
            var newLetNode = new SyntaxStatementLetDeclarator().WithDeclarator(new SyntaxTypeDeclarator().WithName(letName)).WithExpression(letValue);
            var members = new List<SyntaxStatementLet>() { newLetNode };

            switch (pattern)
            {
                case SyntaxDestructuringArrayPattern arrayPattern:
                {
                    int index = 0;
                    foreach (var alias in arrayPattern.Aliases)
                    {
                        alias
                            .IfLeft(name => members.Add(
                                new SyntaxStatementLetDeclarator()
                                    .WithDeclarator(new SyntaxTypeDeclarator()
                                        .WithName(name))
                                    .WithExpression(new SyntaxExpressionMember()
                                        .WithCallee(new SyntaxExpressionLiteralIdentifier().WithName(letName))
                                        .WithIndex(new SyntaxExpressionLiteralDouble().WithValue(index)))));
                        alias
                            .IfRight(innerPattern => members.AddRange(
                                TransformDestructuringLetStatement(context, innerPattern, 
                                    new SyntaxExpressionMember()
                                        .WithCallee(new SyntaxExpressionLiteralIdentifier().WithName(letName))
                                        .WithIndex(new SyntaxExpressionLiteralDouble().WithValue(index)))));

                        index += 1;
                    }

                    break;
                }
                case SyntaxDestructuringObjectPattern objectPattern:
                {
                    foreach (var binding in objectPattern.BindingElements)
                    {
                        binding.Alias
                            .IfLeft(name => members.Add(
                                new SyntaxStatementLetDeclarator()
                                    .WithDeclarator(new SyntaxTypeDeclarator().WithName(name))
                                    .WithExpression(new SyntaxExpressionProperty()
                                        .WithLeft(new SyntaxExpressionLiteralIdentifier()
                                            .WithName(letName))
                                        .WithName(name))));
                        binding.Alias
                            .IfRight(innerPattern => members.AddRange(
                                TransformDestructuringLetStatement(context, innerPattern, 
                                    new SyntaxExpressionProperty()
                                        .WithLeft(new SyntaxExpressionLiteralIdentifier()
                                            .WithName(letName))
                                        .WithName(binding.Property))));
                    }

                    break;
                }
                case SyntaxDestructuringTuplePatternNode tuplePattern:
                {
                    int index = 0;
                    foreach (var alias in tuplePattern.Aliases)
                    {
                        alias
                            .IfLeft(name => members.Add(
                                new SyntaxStatementLetDeclarator()
                                    .WithDeclarator(new SyntaxTypeDeclarator()
                                        .WithName(name))
                                    .WithExpression(new SyntaxExpressionMember()
                                        .WithCallee(new SyntaxExpressionLiteralIdentifier().WithName(letName))
                                        .WithIndex(new SyntaxExpressionLiteralDouble().WithValue(index)))));
                        alias
                            .IfRight(innerPattern => members.AddRange(
                                TransformDestructuringLetStatement(context, innerPattern, 
                                    new SyntaxExpressionMember()
                                        .WithCallee(new SyntaxExpressionLiteralIdentifier().WithName(letName))
                                        .WithIndex(new SyntaxExpressionLiteralDouble().WithValue(index)))));

                        index += 1;
                    }

                    break;
                }
            }

            return members.ToArray();
        }

        public T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T) formatter.Deserialize(ms);
            }
        }
    }
}