using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast;

namespace Nord.Compiler.Generated.Ast.Nodes
{
    public class SyntaxStatement : SyntaxNode
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatement)(base.Copy());
            return copy;
        }
    }
}