using System;
using System.Collections.Generic;
using LanguageExt;

namespace Nord.Compiler.Generated.Ast
{
    public class SyntaxNode
    {
        public virtual SyntaxNode Copy()
        {
            var copy = (SyntaxNode)(new SyntaxNode());
            return copy;
        }
    }
}