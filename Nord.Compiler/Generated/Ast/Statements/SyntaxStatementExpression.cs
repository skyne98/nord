using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast;

namespace Nord.Compiler.Generated.Ast.Statements
{
    public class SyntaxStatementExpression : SyntaxStatement
    {
        public string Expression
        {
            get;
            private set;
        }

        public override SyntaxStatement Copy()
        {
            var copy = (SyntaxStatementExpression)(base.Copy());
            copy.Expression = this.Expression;
            return copy;
        }

        SyntaxStatementExpression WithExpression(string value)
        {
            var copy = (SyntaxStatementExpression)(this.Copy());
            copy.Expression = value;
            return copy;
        }
    }
}