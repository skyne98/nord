using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Types
{
    public class SyntaxTypeParameter : SyntaxType
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
            var copy = (SyntaxTypeParameter)(base.Copy());
            copy.Name = this.Name;
            copy.Constraint = this.Constraint;
            return copy;
        }

        SyntaxTypeParameter WithName(string value)
        {
            var copy = (SyntaxTypeParameter)(this.Copy());
            copy.Name = value;
            return copy;
        }

        SyntaxTypeParameter WithConstraint(SyntaxTypeReference value)
        {
            var copy = (SyntaxTypeParameter)(this.Copy());
            copy.Constraint = value;
            return copy;
        }
    }
}