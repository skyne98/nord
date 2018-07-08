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
        public static TokenListParser<NordTokenType, AstExpressionLiteralNode<string>> String { get; } =
            from token in Token.EqualTo(NordTokenType.String)
            select new AstExpressionLiteralNode<string>(token.Span.ToStringValue().Substring(1, token.Span.ToStringValue().Length - 2));

        public static TokenListParser<NordTokenType, AstExpressionLiteralNode<double>> Double { get; } =
            from token in Token.EqualTo(NordTokenType.Double)
            select new AstExpressionLiteralNode<double>(double.Parse(token.Span.ToStringValue()));
    }
}
