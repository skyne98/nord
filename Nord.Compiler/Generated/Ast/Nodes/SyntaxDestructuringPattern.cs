using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast;

namespace Nord.Compiler.Generated.Ast.Nodes
{
    public class SyntaxDestructuringPattern : SyntaxNode
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxDestructuringPattern)(base.Copy());
            return copy;
        }
    }
}