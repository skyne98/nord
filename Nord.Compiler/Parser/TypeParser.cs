using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.SomeHelp;
using Nord.Compiler.Ast;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Types;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class TypeParser
    {
        public static TokenListParser<TokenType, SyntaxTypeReference[]> TypeArguments { get; } =
            from openAngle in Token.EqualTo(TokenType.OpenAngle)
            from arguments in Parse.Ref(() => TypeReference).ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
            from closeAngle in Token.EqualTo(TokenType.CloseAngle)
            select arguments;

        public static TokenListParser<TokenType, SyntaxTypeReference> TypeReference { get; } =
            from identifier in Token.EqualTo(TokenType.Identifier)
            from arguments in TypeArguments.OptionalOrDefault()
            select arguments != null && arguments.Length > 0
                ? new SyntaxTypeReference()
                    .WithName(identifier.ToStringValue())
                    .WithArguments(arguments)
                : new SyntaxTypeReference()
                    .WithName(identifier.ToStringValue());
        
        public static TokenListParser<TokenType, SyntaxTypeParameter[]> TypeParameters { get; } =
            from openAngle in Token.EqualTo(TokenType.OpenAngle)
            from arguments in Parse.Ref(() => TypeParameter).ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
            from closeAngle in Token.EqualTo(TokenType.CloseAngle)
            select arguments;

        public static TokenListParser<TokenType, SyntaxTypeParameter> TypeParameter { get; } =
            from identifier in Token.EqualTo(TokenType.Identifier)
            from constraint in (
                from colon in Token.EqualTo(TokenType.Colon)
                from reference in TypeReference
                select reference
            ).OptionalOrDefault()
            select new SyntaxTypeParameter()
                .WithName(identifier.ToStringValue())
                .WithConstraint(constraint);

        public static TokenListParser<TokenType, SyntaxTypeDeclarator> Declarator { get; } =
            from identifier in Token.EqualTo(TokenType.Identifier)
            from type in (
                from colon in Token.EqualTo(TokenType.Colon)
                from type in TypeReference
                select type
            ).OptionalOrDefault()
            select new SyntaxTypeDeclarator()
                .WithName(identifier.ToStringValue())
                .WithType(type);
    }
}
