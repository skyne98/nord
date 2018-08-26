using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Statements;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.StatementLets
{
    public class SyntaxStatementLetDestructuring : SyntaxStatementLet
    {
        public SyntaxDestructuringPattern Pattern
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementLetDestructuring)(base.Copy());
            copy.Pattern = this.Pattern;
            return copy;
        }

        SyntaxStatementLetDestructuring WithPattern(SyntaxDestructuringPattern value)
        {
            var copy = (SyntaxStatementLetDestructuring)(this.Copy());
            copy.Pattern = value;
            return copy;
        }
    }
}