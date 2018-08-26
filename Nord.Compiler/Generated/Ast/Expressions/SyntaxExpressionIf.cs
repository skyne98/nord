using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Expressions
{
    public class SyntaxExpressionIf : SyntaxExpression
    {
        public SyntaxExpression Condition
        {
            get;
            private set;
        }

        public SyntaxExpression Then
        {
            get;
            private set;
        }

        public SyntaxExpression Else
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionIf)(base.Copy());
            copy.Condition = this.Condition;
            copy.Then = this.Then;
            copy.Else = this.Else;
            return copy;
        }

        SyntaxExpressionIf WithCondition(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionIf)(this.Copy());
            copy.Condition = value;
            return copy;
        }

        SyntaxExpressionIf WithThen(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionIf)(this.Copy());
            copy.Then = value;
            return copy;
        }

        SyntaxExpressionIf WithElse(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionIf)(this.Copy());
            copy.Else = value;
            return copy;
        }
    }
}