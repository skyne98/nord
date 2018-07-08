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

        public static TokenListParser<TokenType, AstStatementLoopNode> LoopStatement { get; } =
            from loopKeyword in Token.EqualTo(TokenType.LoopKeyword)
            from curlyOpen in Token.EqualTo(TokenType.OpenCurly)
            from body in Parsers.StatementsBlock
            from curlyClose in Token.EqualTo(TokenType.CloseCurly)
            select new AstStatementLoopNode(body);

        public static TokenListParser<TokenType, AstStatementNode> Statement { get; } =
            LoopStatement.Select(s => (AstStatementNode)s)
                .Or(ExpressionStatement.Select(e => (AstStatementNode)e));
    }
}
