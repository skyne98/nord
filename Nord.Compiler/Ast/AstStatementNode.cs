using System;
using System.Collections.Generic;
using System.Text;

namespace Nord.Compiler.Ast
{
    public class AstStatementNode: AstNode
    {

    }

    public class AstStatementExpressionNode : AstStatementNode
    {
        public AstStatementExpressionNode(AstExpressionNode expression)
        {
            Expression = expression;
        }

        public AstExpressionNode Expression { get; private set; }
    }

    // Loops
    public class AstStatementLoopNode: AstStatementNode
    {
        public AstStatementLoopNode(AstStatementNode[] statements)
        {
            Statements = statements;
        }

        public AstStatementNode[] Statements { get; private set; }
    }
}