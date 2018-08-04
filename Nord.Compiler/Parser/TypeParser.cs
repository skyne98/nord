using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.SomeHelp;
using Nord.Compiler.Ast;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class TypeParser
    {
        public static TokenListParser<TokenType, AstTypeReferenceNode[]> TypeArguments { get; } =
            from openAngle in Token.EqualTo(TokenType.OpenAngle)
            from arguments in Parse.Ref(() => TypeReference).ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
            from closeAngle in Token.EqualTo(TokenType.CloseAngle)
            select arguments;

        public static TokenListParser<TokenType, AstTypeReferenceNode> TypeReference { get; } =
            from identifier in Token.EqualTo(TokenType.Identifier)
            from arguments in TypeArguments.OptionalOrDefault()
            select arguments != null && arguments.Length > 0
                ? new AstTypeReferenceNode(identifier.ToStringValue(), arguments)
                : new AstTypeReferenceNode(identifier.ToStringValue(), new AstTypeReferenceNode[]{});
        
        public static TokenListParser<TokenType, AstTypeParameterNode[]> TypeParameters { get; } =
            from openAngle in Token.EqualTo(TokenType.OpenAngle)
            from arguments in Parse.Ref(() => TypeParameter).ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
            from closeAngle in Token.EqualTo(TokenType.CloseAngle)
            select arguments;

        public static TokenListParser<TokenType, AstTypeParameterNode> TypeParameter { get; } =
            from identifier in Token.EqualTo(TokenType.Identifier)
            from constraint in (
                from colon in Token.EqualTo(TokenType.Colon)
                from reference in TypeReference
                select reference
            ).OptionalOrDefault()
            select new AstTypeParameterNode(identifier.ToStringValue(), constraint);

        public static TokenListParser<TokenType, AstTypeDeclaratorNode> Declarator { get; } =
            from identifier in Token.EqualTo(TokenType.Identifier)
            from type in (
                from colon in Token.EqualTo(TokenType.Colon)
                from type in TypeReference
                select type
            ).OptionalOrDefault()
            select new AstTypeDeclaratorNode(identifier.ToStringValue(), type);
    }
}
