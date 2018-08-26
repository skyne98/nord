using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Expressions
{
    public class SyntaxExpressionUnary : SyntaxExpression
    {
        public string Operator
        {
            get;
            private set;
        }

        public SyntaxExpression Value
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionUnary)(base.Copy());
            copy.Operator = this.Operator;
            copy.Value = this.Value;
            return copy;
        }

        SyntaxExpressionUnary WithOperator(string value)
        {
            var copy = (SyntaxExpressionUnary)(this.Copy());
            copy.Operator = value;
            return copy;
        }

        SyntaxExpressionUnary WithValue(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionUnary)(this.Copy());
            copy.Value = value;
            return copy;
        }
    }
}