using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Statements
{
    public class SyntaxStatementLet : SyntaxStatement
    {
        public SyntaxExpression Expression
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementLet)(base.Copy());
            copy.Expression = this.Expression;
            return copy;
        }

        SyntaxStatementLet WithExpression(SyntaxExpression value)
        {
            var copy = (SyntaxStatementLet)(this.Copy());
            copy.Expression = value;
            return copy;
        }
    }
}