using System;
using System.Collections.Generic;
using System.Text;
using Nord.Compiler.Ast;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class StatementParser
    {
        public static TokenListParser<TokenType, AstStatementExpressionNode> ExpressionStatement { get; } =
            from expression in ExpressionParser.Expression
            select new AstStatementExpressionNode(expression);

        public static TokenListParser<TokenType, AstStatementLetNode> LetStatement { get; } =
            from letKeyword in Token.EqualTo(TokenType.LetKeyword)
            from declarator in TypeParser.Declarator
            from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
            from value in ExpressionParser.Expression
            select new AstStatementLetNode(declarator, value);

        public static TokenListParser<TokenType, AstStatementReturnNode> ReturnStatement { get; } =
            from returnKeywork in Token.EqualTo(TokenType.ReturnKeyword)
            from value in ExpressionParser.Expression
            select new AstStatementReturnNode(value);

        public static TokenListParser<TokenType, AstStatementBreakNode> BreakStatement { get; } =
            from breakKeywork in Token.EqualTo(TokenType.BreakKeyword)
            select new AstStatementBreakNode();

        public static TokenListParser<TokenType, AstStatementBreakNode> ContinueStatement { get; } =
            from breakKeywork in Token.EqualTo(TokenType.ContinueKeyword)
            select new AstStatementBreakNode();

        public static TokenListParser<TokenType, AstStatementLoopNode> LoopStatement { get; } =
            from loopKeyword in Token.EqualTo(TokenType.LoopKeyword)
            from curlyOpen in Token.EqualTo(TokenType.OpenCurly)
            from body in Parsers.StatementsBlock
            from curlyClose in Token.EqualTo(TokenType.CloseCurly)
            select new AstStatementLoopNode(body);

        public static TokenListParser<TokenType, AstStatementNode> Statement { get; } =
            LoopStatement.Select(s => (AstStatementNode) s)
                .Or(LetStatement.Select(l => (AstStatementNode) l))
                .Or(ReturnStatement.Select(r => (AstStatementNode) r))
                .Or(BreakStatement.Select(b => (AstStatementNode) b))
                .Or(ContinueStatement.Select(c => (AstStatementNode) c))
                .Or(ExpressionStatement.Select(e => (AstStatementNode) e));
    }
}
