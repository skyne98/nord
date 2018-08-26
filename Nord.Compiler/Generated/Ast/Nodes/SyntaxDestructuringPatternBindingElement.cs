using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast;

namespace Nord.Compiler.Generated.Ast.Nodes
{
    public class SyntaxDestructuringPatternBindingElement : SyntaxNode
    {
        public Either<string, SyntaxDestructuringPattern> Alias
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxDestructuringPatternBindingElement)(base.Copy());
            copy.Alias = this.Alias;
            return copy;
        }

        SyntaxDestructuringPatternBindingElement WithAlias(Either<string, SyntaxDestructuringPattern> value)
        {
            var copy = (SyntaxDestructuringPatternBindingElement)(this.Copy());
            copy.Alias = value;
            return copy;
        }
    }
}