using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Expressions;

namespace Nord.Compiler.Generated.Ast.ExpressionLiterals
{
    public class SyntaxExpressionLiteralFloat : SyntaxExpressionLiteral
    {
        public float Value
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionLiteralFloat)(base.Copy());
            copy.Value = this.Value;
            return copy;
        }

        SyntaxExpressionLiteralFloat WithValue(float value)
        {
            var copy = (SyntaxExpressionLiteralFloat)(this.Copy());
            copy.Value = value;
            return copy;
        }
    }
}