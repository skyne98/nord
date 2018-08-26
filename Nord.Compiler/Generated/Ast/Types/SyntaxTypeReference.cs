using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Types
{
    public class SyntaxTypeReference : SyntaxType
    {
        public string Name
        {
            get;
            private set;
        }

        public SyntaxTypeReference Constraint
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxTypeReference)(base.Copy());
            copy.Name = this.Name;
            copy.Constraint = this.Constraint;
            return copy;
        }

        SyntaxTypeReference WithName(string value)
        {
            var copy = (SyntaxTypeReference)(this.Copy());
            copy.Name = value;
            return copy;
        }

        SyntaxTypeReference WithConstraint(SyntaxTypeReference value)
        {
            var copy = (SyntaxTypeReference)(this.Copy());
            copy.Constraint = value;
            return copy;
        }
    }
}