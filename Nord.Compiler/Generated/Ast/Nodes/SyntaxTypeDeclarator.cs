using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast;

namespace Nord.Compiler.Generated.Ast.Nodes
{
    public class SyntaxTypeDeclarator : SyntaxNode
    {
        public string Name
        {
            get;
            private set;
        }

        public SyntaxType Type
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxTypeDeclarator)(base.Copy());
            copy.Name = this.Name;
            copy.Type = this.Type;
            return copy;
        }

        SyntaxTypeDeclarator WithName(string value)
        {
            var copy = (SyntaxTypeDeclarator)(this.Copy());
            copy.Name = value;
            return copy;
        }

        SyntaxTypeDeclarator WithType(SyntaxType value)
        {
            var copy = (SyntaxTypeDeclarator)(this.Copy());
            copy.Type = value;
            return copy;
        }
    }
}