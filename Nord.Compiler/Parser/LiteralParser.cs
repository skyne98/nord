using System;
using System.Collections.Generic;
using System.Text;
using Nord.Compiler.Generated.Ast.ExpressionLiterals;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class LiteralParser
    {
        public static TokenListParser<TokenType, SyntaxExpressionLiteralString> String { get; } =
            from token in Token.EqualTo(TokenType.String)
            select new SyntaxExpressionLiteralString()
                .WithValue(token.Span.ToStringValue().Substring(1, token.Span.ToStringValue().Length - 2));

        public static TokenListParser<TokenType, SyntaxExpressionLiteralDouble> Double { get; } =
            from token in Token.EqualTo(TokenType.Double)
            select new SyntaxExpressionLiteralDouble()
                .WithValue(double.Parse(token.Span.ToStringValue()));

        public static TokenListParser<TokenType, SyntaxExpressionLiteralIdentifier> Identifier { get; } =
            from name in Token.EqualTo(TokenType.Identifier)
            select new SyntaxExpressionLiteralIdentifier()
                .WithName(name.Span.ToStringValue());
    }
}
