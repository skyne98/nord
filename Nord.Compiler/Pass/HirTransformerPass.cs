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

namespace Nord.Compiler.Pass
{
    public class HirTransformerPass : ICompilerPass
    {
        public AstNode Run(Context context)
        {
            var root = DeepClone(context.Ast);
            return Transform(context, root).FirstOrDefault();
        }

        private AstNode[] Transform(Context context, AstNode node)
        {
            switch (node)
            {
                case AstDocumentNode documentNode:
                    return new[] {new AstDocumentNode(documentNode.Statements
                        .SelectMany(st => Transform(context, st)).Cast<AstStatementTopLevelNode>().ToArray())};
                case AstStatementNode statementNode:
                    return TransformStatement(context, statementNode);
            }
            
            return new AstNode[] { DeepClone(node) };
        } 
        
        private AstNode[] TransformStatement(Context context, AstStatementNode node)
        {
            switch (node)
            {
                case AstStatementLetNode letNode:
                {
                    if (letNode.Declarator.IsRight)
                    {
                        return TransformDestructuringLetStatement(context, (AstDestructuringPatternNode) letNode.Declarator, letNode.Value)
                            .Cast<AstNode>().ToArray();
                    }
                    break;
                }
                case AstStatementTopLevelNode topLevelNode:
                {
                    return new AstNode[]
                    {
                        new AstStatementTopLevelNode(topLevelNode.Modifiers,
                            Transform(context, topLevelNode.Statement)
                                .FirstOrDefault() as AstStatementNode)
                    };
                }
                case AstStatementFunctionNode functionNode:
                {
                    return new[] { new AstStatementFunctionNode(functionNode.Name, 
                        functionNode.TypeParameters, 
                        functionNode.Parameters,
                        functionNode.Return,
                        functionNode.Body.SelectMany(bs => Transform(context, bs))
                            .Cast<AstStatementNode>()
                            .ToArray())};
                }
            }

            return new AstNode[] { DeepClone(node) };
        }

        private AstStatementLetNode[] TransformDestructuringLetStatement(Context context, AstDestructuringPatternNode pattern,
            AstExpressionNode value)
        {
            var letName = context.GenerateVariableName();
            var letValue = Transform(context, value).FirstOrDefault() as AstExpressionNode;
            var newLetNode = new AstStatementLetNode(new AstTypeDeclaratorNode(letName), letValue);
            var members = new List<AstStatementLetNode>() { newLetNode };

            switch (pattern)
            {
                case AstDestructuringArrayPatternNode arrayPattern:
                {
                    int index = 0;
                    foreach (var alias in arrayPattern.Aliases)
                    {
                        alias
                            .IfLeft(name => members.Add(
                                new AstStatementLetNode(
                                    new AstTypeDeclaratorNode(name),
                                    new AstExpressionMemberNode(
                                        new AstExpressionIdentifierLiteralNode(letName), 
                                        new AstExpressionLiteralNode<double>(index)))));
                        alias
                            .IfRight(innerPattern => members.AddRange(
                                TransformDestructuringLetStatement(context, innerPattern, 
                                    new AstExpressionMemberNode(
                                        new AstExpressionIdentifierLiteralNode(letName), 
                                        new AstExpressionLiteralNode<double>(index)))
                                    .Cast<AstStatementLetNode>()));

                        index += 1;
                    }

                    break;
                }
                case AstDestructuringObjectPetternNode objectPattern:
                {
                    foreach (var binding in objectPattern.BindingElements)
                    {
                        binding.Alias
                            .IfLeft(name => members.Add(
                                new AstStatementLetNode(
                                    new AstTypeDeclaratorNode(name),
                                    new AstExpressionPropertyAccessNode(
                                        new AstExpressionIdentifierLiteralNode(letName), 
                                        binding.Name))));
                        binding.Alias
                            .IfRight(innerPattern => members.AddRange(
                                TransformDestructuringLetStatement(context, innerPattern, 
                                        new AstExpressionPropertyAccessNode(
                                            new AstExpressionIdentifierLiteralNode(letName), 
                                            binding.Name))
                                    .Cast<AstStatementLetNode>()));
                    }

                    break;
                }
                case AstDestructuringTuplePatternNode tuplePattern:
                {
                    int index = 0;
                    foreach (var alias in tuplePattern.Aliases)
                    {
                        alias
                            .IfLeft(name => members.Add(
                                new AstStatementLetNode(
                                    new AstTypeDeclaratorNode(name),
                                    new AstExpressionMemberNode(
                                        new AstExpressionIdentifierLiteralNode(letName), 
                                        new AstExpressionLiteralNode<double>(index)))));
                        alias
                            .IfRight(innerPattern => members.AddRange(
                                TransformDestructuringLetStatement(context, innerPattern, 
                                        new AstExpressionMemberNode(
                                            new AstExpressionIdentifierLiteralNode(letName), 
                                            new AstExpressionLiteralNode<double>(index)))
                                    .Cast<AstStatementLetNode>()));

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