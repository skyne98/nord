using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast;

namespace Nord.Compiler.Generated.Ast.Nodes
{
    public class SyntaxModifier : SyntaxNode
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxModifier)(base.Copy());
            return copy;
        }
    }
}