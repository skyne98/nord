using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Statements
{
    public class SyntaxStatementUse : SyntaxStatement
    {
        public string From
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementUse)(base.Copy());
            copy.From = this.From;
            return copy;
        }

        SyntaxStatementUse WithFrom(string value)
        {
            var copy = (SyntaxStatementUse)(this.Copy());
            copy.From = value;
            return copy;
        }
    }
}