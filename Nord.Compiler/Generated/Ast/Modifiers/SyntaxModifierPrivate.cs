using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Modifiers
{
    public class SyntaxModifierPrivate : SyntaxModifier
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxModifierPrivate)(base.Copy());
            return copy;
        }
    }
}