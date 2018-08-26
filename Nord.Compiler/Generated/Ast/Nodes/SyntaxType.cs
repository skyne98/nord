using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast;

namespace Nord.Compiler.Generated.Ast.Nodes
{
    public class SyntaxType : SyntaxNode
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxType)(base.Copy());
            return copy;
        }
    }
}