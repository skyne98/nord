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

    public class AstStatementLetNode : AstStatementNode
    {
        public AstStatementLetNode(AstTypeDeclaratorNode declarator, AstExpressionNode value)
        {
            Declarator = declarator;
            Value = value;
        }

        public AstTypeDeclaratorNode Declarator { get; private set; }
        public AstExpressionNode Value { get; private set; }
    }

    public class AstStatementReturnNode : AstStatementNode
    {
        public AstStatementReturnNode(AstExpressionNode value)
        {
            Value = value;
        }

        public AstExpressionNode Value { get; private set; }
    }

    public class AstStatementBreakNode : AstStatementNode
    {

    }

    public class AstStatementContinueNode : AstStatementNode
    {

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