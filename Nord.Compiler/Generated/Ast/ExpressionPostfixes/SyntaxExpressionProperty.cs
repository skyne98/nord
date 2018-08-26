using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Expressions;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.ExpressionPostfixes
{
    public class SyntaxExpressionProperty : SyntaxExpressionPostfix
    {
        public SyntaxExpression Left
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpressionProperty)(base.Copy());
            copy.Left = this.Left;
            copy.Name = this.Name;
            return copy;
        }

        SyntaxExpressionProperty WithLeft(SyntaxExpression value)
        {
            var copy = (SyntaxExpressionProperty)(this.Copy());
            copy.Left = value;
            return copy;
        }

        SyntaxExpressionProperty WithName(string value)
        {
            var copy = (SyntaxExpressionProperty)(this.Copy());
            copy.Name = value;
            return copy;
        }
    }
}