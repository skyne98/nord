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
        public static TokenListParser<NordTokenType, AstStatementExpressionNode> ExpressionStatement { get; } =
            from expression in ExpressionParser.Expression
            select new AstStatementExpressionNode(expression);

        public static TokenListParser<NordTokenType, AstStatementNode> Statement { get; } =
            ExpressionStatement.Select(e => (AstStatementNode)e);
    }
}
