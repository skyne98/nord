using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast;
using Nord.Compiler.Generated.Ast.Statements;

namespace Nord.Compiler.Generated.Ast.Nodes
{
    public class SyntaxDocument : SyntaxNode
    {
        public SyntaxStatementTopLevel[] Statements
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxDocument)(base.Copy());
            copy.Statements = this.Statements;
            return copy;
        }

        SyntaxDocument WithStatements(SyntaxStatementTopLevel[] value)
        {
            var copy = (SyntaxDocument)(this.Copy());
            copy.Statements = value;
            return copy;
        }
    }
}