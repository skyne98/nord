using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Statements
{
    public class SyntaxStatementFunction : SyntaxStatement
    {
        public string Name
        {
            get;
            private set;
        }

        public SyntaxTypeParameter[] TypeParameters
        {
            get;
            private set;
        }

        public SyntaxTypeDeclarator[] Parameters
        {
            get;
            private set;
        }

        public Option<SyntaxTypeReference> Return
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementFunction)(base.Copy());
            copy.Name = this.Name;
            copy.TypeParameters = this.TypeParameters;
            copy.Parameters = this.Parameters;
            copy.Return = this.Return;
            return copy;
        }

        SyntaxStatementFunction WithName(string value)
        {
            var copy = (SyntaxStatementFunction)(this.Copy());
            copy.Name = value;
            return copy;
        }

        SyntaxStatementFunction WithTypeParameters(SyntaxTypeParameter[] value)
        {
            var copy = (SyntaxStatementFunction)(this.Copy());
            copy.TypeParameters = value;
            return copy;
        }

        SyntaxStatementFunction WithParameters(SyntaxTypeDeclarator[] value)
        {
            var copy = (SyntaxStatementFunction)(this.Copy());
            copy.Parameters = value;
            return copy;
        }

        SyntaxStatementFunction WithReturn(Option<SyntaxTypeReference> value)
        {
            var copy = (SyntaxStatementFunction)(this.Copy());
            copy.Return = value;
            return copy;
        }
    }
}