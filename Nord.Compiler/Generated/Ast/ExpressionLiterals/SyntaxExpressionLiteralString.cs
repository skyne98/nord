using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Expressions;

namespace Nord.Compiler.Generated.Ast.ExpressionLiterals
{
    public class SyntaxExpressionLiteralString : SyntaxExpressionLiteral
    {
        public string Value
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionLiteralString)(base.Copy());
            copy.Value = this.Value;
            return copy;
        }

        SyntaxExpressionLiteralString WithValue(string value)
        {
            var copy = (SyntaxExpressionLiteralString)(this.Copy());
            copy.Value = value;
            return copy;
        }
    }
}