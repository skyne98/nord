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
            LiteralParser.String.Select(e => (AstExpressionNode) e)
                .Or(LiteralParser.Double.Select(e => (AstExpressionNode) e))
                .Or(LiteralParser.Identifier.Select(e => (AstExpressionNode) e));

        public static TokenListParser<TokenType, AstExpressionLetNode> Let { get; } =
            from letKeyword in Token.EqualTo(TokenType.LetKeyword)
            from declarator in TypeParser.Declarator
            from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
            from value in ExpressionParser.Expression
            select new AstExpressionLetNode(declarator, value);

        public static TokenListParser<TokenType, AstExpressionNode> Terminal { get; } =
            (
                from openParen in Token.EqualTo(TokenType.OpenParen)
                from expression in Parse.Ref(() => Expression)
                from closeParen in Token.EqualTo(TokenType.CloseParen)
                select expression
            )
            .Or(Literal);
        
        public static TokenListParser<TokenType, AstExpressionFunctionCallNode> FunctionCall { get; } =
            from function in Terminal
            from openParen in Token.EqualTo(TokenType.OpenParen)
            from parameters in (
                from expressions in Parse.Ref(() => Expression).ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
                select expressions
            )
            from closeParen in Token.EqualTo(TokenType.CloseParen)
            select new AstExpressionFunctionCallNode(function, parameters);

        public static TokenListParser<TokenType, AstExpressionIndexedAccessNode> IndexedAccess { get; } =
            from subject in Terminal
            from openSquare in Token.EqualTo(TokenType.OpenSquare)
            from index in Parse.Ref(() => Expression)
            from closeSquare in Token.EqualTo(TokenType.CloseSquare)
            select new AstExpressionIndexedAccessNode(subject, index);

        public static TokenListParser<TokenType, AstExpressionNode> Expression { get; } =
            Let.Select(e => (AstExpressionNode)e)
                .Or(Literal);
    }
}
