using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Modifiers
{
    public class SyntaxModifierPublic : SyntaxModifier
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxModifierPublic)(base.Copy());
            return copy;
        }
    }
}