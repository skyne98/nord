using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.Statements
{
    public class SyntaxStatementContinue : SyntaxStatement
    {
        public override SyntaxNode Copy()
        {
            var copy = (SyntaxStatementContinue)(base.Copy());
            return copy;
        }
    }
}