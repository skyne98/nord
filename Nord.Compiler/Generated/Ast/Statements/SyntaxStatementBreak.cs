using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Statements
{
    public class SyntaxStatementBreak : SyntaxStatement
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementBreak)(base.Copy());
            return copy;
        }
    }
}