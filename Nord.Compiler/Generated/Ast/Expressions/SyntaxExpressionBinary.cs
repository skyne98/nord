using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Expressions
{
    public class SyntaxExpressionBinary : SyntaxExpression
    {
        public string Operator
        {
            get;
            private set;
        }

        public SyntaxExpression Left
        {
            get;
            private set;
        }

        public SyntaxExpression Right
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionBinary)(base.Copy());
            copy.Operator = this.Operator;
            copy.Left = this.Left;
            copy.Right = this.Right;
            return copy;
        }

        SyntaxExpressionBinary WithOperator(string value)
        {
            var copy = (SyntaxExpressionBinary)(this.Copy());
            copy.Operator = value;
            return copy;
        }

        SyntaxExpressionBinary WithLeft(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionBinary)(this.Copy());
            copy.Left = value;
            return copy;
        }

        SyntaxExpressionBinary WithRight(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionBinary)(this.Copy());
            copy.Right = value;
            return copy;
        }
    }
}