using System;
using System.Collections.Generic;
using System.Text;
using Nord.Compiler.Ast;
using Nord.Compiler.Lexer;
using Superpower;

namespace Nord.Compiler.Parser
{
    public class ExpressionParser
    { 
        public static TokenListParser<NordTokenType, AstExpressionNode> Literal { get; } =
            LiteralParser.String.Select(e => (AstExpressionNode)e)
                .Or(LiteralParser.Double.Select(e => (AstExpressionNode) e));

        public static TokenListParser<NordTokenType, AstExpressionNode> Expression { get; } =
            Literal;
    }
}
