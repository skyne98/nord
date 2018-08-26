using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Types;

namespace Nord.Compiler.Generated.Ast.Expressions
{
    public class SyntaxExpressionAs : SyntaxExpression
    {
        public SyntaxExpression Expression
        {
            get;
            private set;
        }

        public SyntaxTypeReference Type
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionAs)(base.Copy());
            copy.Expression = this.Expression;
            copy.Type = this.Type;
            return copy;
        }

        SyntaxExpressionAs WithExpression(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionAs)(this.Copy());
            copy.Expression = value;
            return copy;
        }

        SyntaxExpressionAs WithType(SyntaxTypeReference value)
        {
            var copy = (SyntaxExpressionAs)(this.Copy());
            copy.Type = value;
            return copy;
        }
    }
}