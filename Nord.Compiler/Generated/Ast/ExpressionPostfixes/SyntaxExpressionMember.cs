using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Expressions;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.ExpressionPostfixes
{
    public class SyntaxExpressionMember : SyntaxExpressionPostfix
    {
        public SyntaxExpression Callee
        {
            get;
            private set;
        }

        public SyntaxExpression Index
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionMember)(base.Copy());
            copy.Callee = this.Callee;
            copy.Index = this.Index;
            return copy;
        }

        SyntaxExpressionMember WithCallee(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionMember)(this.Copy());
            copy.Callee = value;
            return copy;
        }

        SyntaxExpressionMember WithIndex(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionMember)(this.Copy());
            copy.Index = value;
            return copy;
        }
    }
}