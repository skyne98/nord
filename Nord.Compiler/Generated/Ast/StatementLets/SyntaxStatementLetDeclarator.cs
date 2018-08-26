using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Statements;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.StatementLets
{
    public class SyntaxStatementLetDeclarator : SyntaxStatementLet
    {
        public SyntaxTypeDeclarator Declarator
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementLetDeclarator)(base.Copy());
            copy.Declarator = this.Declarator;
            return copy;
        }

        SyntaxStatementLetDeclarator WithDeclarator(SyntaxTypeDeclarator value)
        {
            var copy = (SyntaxStatementLetDeclarator)(this.Copy());
            copy.Declarator = value;
            return copy;
        }
    }
}