using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Expressions;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Types;

namespace Nord.Compiler.Generated.Ast.ExpressionPostfixes
{
    public class SyntaxExpressionCall : SyntaxExpressionPostfix
    {
        public SyntaxExpression Callee
        {
            get;
            private set;
        }

        public SyntaxTypeReference[] TypeParameters
        {
            get;
            private set;
        }

        public SyntaxExpression[] Parameters
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionCall)(base.Copy());
            copy.Callee = this.Callee;
            copy.TypeParameters = this.TypeParameters;
            copy.Parameters = this.Parameters;
            return copy;
        }

        SyntaxExpressionCall WithCallee(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionCall)(this.Copy());
            copy.Callee = value;
            return copy;
        }

        SyntaxExpressionCall WithTypeParameters(SyntaxTypeReference[] value)
        {
            var copy = (SyntaxExpressionCall)(this.Copy());
            copy.TypeParameters = value;
            return copy;
        }

        SyntaxExpressionCall WithParameters(SyntaxExpression[] value)
        {
            var copy = (SyntaxExpressionCall)(this.Copy());
            copy.Parameters = value;
            return copy;
        }
    }
}