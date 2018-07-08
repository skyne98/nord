using System;
using System.Collections.Generic;
using System.Text;
using Nord.Compiler.Ast;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class ExpressionParser
    { 
        public static TokenListParser<TokenType, AstExpressionNode> Literal { get; } =
            LiteralParser.String.Select(e => (AstExpressionNode)e)
                .Or(LiteralParser.Double.Select(e => (AstExpressionNode) e));

        public static TokenListParser<TokenType, AstExpressionLetNode> Let { get; } =
            from letKeyword in Token.EqualTo(TokenType.LetKeyword)
            from declarator in TypeParser.Declarator
            from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
            from value in ExpressionParser.Expression
            select new AstExpressionLetNode(declarator, value);

        public static TokenListParser<TokenType, AstExpressionBinaryNode> BinaryEquality { get; } =
            Parse.Chain(
                (
                    from exclamationMarkOperator in Token.EqualTo(TokenType.ExclamationMarkOperator)
                    from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
                )
            );

        public static TokenListParser<TokenType, AstExpressionNode> Expression { get; } =
            Let.Select(e => (AstExpressionNode)e)
                .Or(Literal);
    }
}
