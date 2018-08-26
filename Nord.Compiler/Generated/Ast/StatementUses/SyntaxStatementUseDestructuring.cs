using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Statements;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.StatementUses
{
    public class SyntaxStatementUseDestructuring : SyntaxStatementUse
    {
        public SyntaxDestructuringPattern Pattern
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementUseDestructuring)(base.Copy());
            copy.Pattern = this.Pattern;
            return copy;
        }

        SyntaxStatementUseDestructuring WithPattern(SyntaxDestructuringPattern value)
        {
            var copy = (SyntaxStatementUseDestructuring)(this.Copy());
            copy.Pattern = value;
            return copy;
        }
    }
}