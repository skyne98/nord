using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Expressions;

namespace Nord.Compiler.Generated.Ast.ExpressionLiterals
{
    public class SyntaxExpressionLiteralIdentifier : SyntaxExpressionLiteral
    {
        public string Name
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionLiteralIdentifier)(base.Copy());
            copy.Name = this.Name;
            return copy;
        }

        SyntaxExpressionLiteralIdentifier WithName(string value)
        {
            var copy = (SyntaxExpressionLiteralIdentifier)(this.Copy());
            copy.Name = value;
            return copy;
        }
    }
}