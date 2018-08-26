using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Statements
{
    public class SyntaxStatementClassDeclaration : SyntaxStatement
    {
        public string Name
        {
            get;
            private set;
        }

        public SyntaxStatementTopLevel[] Body
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementClassDeclaration)(base.Copy());
            copy.Name = this.Name;
            copy.Body = this.Body;
            return copy;
        }

        SyntaxStatementClassDeclaration WithName(string value)
        {
            var copy = (SyntaxStatementClassDeclaration)(this.Copy());
            copy.Name = value;
            return copy;
        }

        SyntaxStatementClassDeclaration WithBody(SyntaxStatementTopLevel[] value)
        {
            var copy = (SyntaxStatementClassDeclaration)(this.Copy());
            copy.Body = value;
            return copy;
        }
    }
}