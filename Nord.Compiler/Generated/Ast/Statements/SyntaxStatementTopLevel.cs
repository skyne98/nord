using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Statements
{
    public class SyntaxStatementTopLevel : SyntaxStatement
    {
        public SyntaxModifier[] Modifiers
        {
            get;
            private set;
        }

        public SyntaxStatement Statement
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementTopLevel)(base.Copy());
            copy.Modifiers = this.Modifiers;
            copy.Statement = this.Statement;
            return copy;
        }

        SyntaxStatementTopLevel WithModifiers(SyntaxModifier[] value)
        {
            var copy = (SyntaxStatementTopLevel)(this.Copy());
            copy.Modifiers = value;
            return copy;
        }

        SyntaxStatementTopLevel WithStatement(SyntaxStatement value)
        {
            var copy = (SyntaxStatementTopLevel)(this.Copy());
            copy.Statement = value;
            return copy;
        }
    }
}