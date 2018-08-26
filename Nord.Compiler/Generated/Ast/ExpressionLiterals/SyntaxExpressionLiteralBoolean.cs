using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Expressions;

namespace Nord.Compiler.Generated.Ast.ExpressionLiterals
{
    public class SyntaxExpressionLiteralBoolean : SyntaxExpressionLiteral
    {
        public bool Value
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionLiteralBoolean)(base.Copy());
            copy.Value = this.Value;
            return copy;
        }

        SyntaxExpressionLiteralBoolean WithValue(bool value)
        {
            var copy = (SyntaxExpressionLiteralBoolean)(this.Copy());
            copy.Value = value;
            return copy;
        }
    }
}