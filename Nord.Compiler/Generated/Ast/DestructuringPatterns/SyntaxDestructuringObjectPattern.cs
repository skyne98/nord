using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.DestructuringPatterns
{
    public class SyntaxDestructuringObjectPattern : SyntaxDestructuringPattern
    {
        public SyntaxDestructuringPatternBindingElement[] BindingElements
        {
            get;
            private set;
        }

        public Option<string> Rest
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxDestructuringObjectPattern)(base.Copy());
            copy.BindingElements = this.BindingElements;
            copy.Rest = this.Rest;
            return copy;
        }

        SyntaxDestructuringObjectPattern WithBindingElements(SyntaxDestructuringPatternBindingElement[] value)
        {
            var copy = (SyntaxDestructuringObjectPattern)(this.Copy());
            copy.BindingElements = value;
            return copy;
        }

        SyntaxDestructuringObjectPattern WithRest(Option<string> value)
        {
            var copy = (SyntaxDestructuringObjectPattern)(this.Copy());
            copy.Rest = value;
            return copy;
        }
    }
}