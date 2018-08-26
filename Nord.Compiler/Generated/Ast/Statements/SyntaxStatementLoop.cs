using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Statements
{
    public class SyntaxStatementLoop : SyntaxStatement
    {
        public SyntaxStatement[] Body
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementLoop)(base.Copy());
            copy.Body = this.Body;
            return copy;
        }

        SyntaxStatementLoop WithBody(SyntaxStatement[] value)
        {
            var copy = (SyntaxStatementLoop)(this.Copy());
            copy.Body = value;
            return copy;
        }
    }
}