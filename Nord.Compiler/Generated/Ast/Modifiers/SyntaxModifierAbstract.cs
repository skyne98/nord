using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Modifiers
{
    public class SyntaxModifierAbstract : SyntaxModifier
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxModifierAbstract)(base.Copy());
            return copy;
        }
    }
}