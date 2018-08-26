using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Modifiers
{
    public class SyntaxModifierFinal : SyntaxModifier
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxModifierFinal)(base.Copy());
            return copy;
        }
    }
}