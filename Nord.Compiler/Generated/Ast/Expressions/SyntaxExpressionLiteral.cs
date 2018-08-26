using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Expressions
{
    public class SyntaxExpressionLiteral : SyntaxExpression
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionLiteral)(base.Copy());
            return copy;
        }
    }
}