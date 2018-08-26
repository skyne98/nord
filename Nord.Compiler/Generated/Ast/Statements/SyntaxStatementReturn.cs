using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Statements
{
    public class SyntaxStatementReturn : SyntaxStatement
    {
        public SyntaxExpression Expression
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementReturn)(base.Copy());
            copy.Expression = this.Expression;
            return copy;
        }

        SyntaxStatementReturn WithExpression(SyntaxExpression value)
        {
            var copy = (SyntaxStatementReturn)(this.Copy());
            copy.Expression = value;
            return copy;
        }
    }
}