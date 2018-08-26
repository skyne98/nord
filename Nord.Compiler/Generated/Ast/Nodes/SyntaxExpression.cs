using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast;

namespace Nord.Compiler.Generated.Ast.Nodes
{
    public class SyntaxExpression : SyntaxNode
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxExpression)(base.Copy());
            return copy;
        }
    }
}