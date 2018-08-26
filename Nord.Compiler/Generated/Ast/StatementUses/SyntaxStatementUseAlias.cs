using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Statements;

namespace Nord.Compiler.Generated.Ast.StatementUses
{
    public class SyntaxStatementUseAlias : SyntaxStatementUse
    {
        public string Alias
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementUseAlias)(base.Copy());
            copy.Alias = this.Alias;
            return copy;
        }

        SyntaxStatementUseAlias WithAlias(string value)
        {
            var copy = (SyntaxStatementUseAlias)(this.Copy());
            copy.Alias = value;
            return copy;
        }
    }
}