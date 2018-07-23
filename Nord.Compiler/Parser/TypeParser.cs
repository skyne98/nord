using System;
using System.Collections.Generic;
using System.Text;
using Nord.Compiler.Ast;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class TypeParser
    {
        public static TokenListParser<TokenType, AstTypeAnnotationNode[]> TypeParameters { get; } =
            from openAngle in Token.EqualTo(TokenType.OpenAngle)
            from parameters in Parse.Ref(() => TypeAnnotation).ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
            from closeAngle in Token.EqualTo(TokenType.CloseAngle)
            select parameters;

        public static TokenListParser<TokenType, AstTypeAnnotationNode> TypeAnnotation { get; } =
            from identifier in Token.EqualTo(TokenType.Identifier)
            from parameters in TypeParameters.OptionalOrDefault()
            select parameters != null && parameters.Length > 0
                ? new AstTypeAnnotationNode(identifier.ToStringValue(), parameters)
                : new AstTypeAnnotationNode(identifier.ToStringValue(), new AstTypeAnnotationNode[]{});

        public static TokenListParser<TokenType, AstTypeDeclaratorNode> Declarator { get; } =
            from identifier in Token.EqualTo(TokenType.Identifier)
            from declaratorType in (
                from colon in Token.EqualTo(TokenType.Colon)
                from declaratorType in TypeAnnotation
                select declaratorType
            ).OptionalOrDefault()
            select new AstTypeDeclaratorNode(identifier.ToStringValue(), declaratorType);
    }
}
