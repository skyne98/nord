using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Modifiers
{
    public class SyntaxModifierOpen : SyntaxModifier
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxModifierOpen)(base.Copy());
            return copy;
        }
    }
}