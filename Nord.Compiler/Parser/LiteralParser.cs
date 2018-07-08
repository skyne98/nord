using System;
using System.Collections.Generic;
using System.Text;
using Nord.Compiler.Ast;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class LiteralParser
    {
        public static TokenListParser<TokenType, AstExpressionLiteralNode<string>> String { get; } =
            from token in Token.EqualTo(TokenType.String)
            select new AstExpressionLiteralNode<string>(token.Span.ToStringValue().Substring(1, token.Span.ToStringValue().Length - 2));

        public static TokenListParser<TokenType, AstExpressionLiteralNode<double>> Double { get; } =
            from token in Token.EqualTo(TokenType.Double)
            select new AstExpressionLiteralNode<double>(double.Parse(token.Span.ToStringValue()));
    }
}
