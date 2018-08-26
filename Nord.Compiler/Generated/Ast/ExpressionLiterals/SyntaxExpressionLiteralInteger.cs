using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Expressions;

namespace Nord.Compiler.Generated.Ast.ExpressionLiterals
{
    public class SyntaxExpressionLiteralInteger : SyntaxExpressionLiteral
    {
        public int Value
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionLiteralInteger)(base.Copy());
            copy.Value = this.Value;
            return copy;
        }

        SyntaxExpressionLiteralInteger WithValue(int value)
        {
            var copy = (SyntaxExpressionLiteralInteger)(this.Copy());
            copy.Value = value;
            return copy;
        }
    }
}