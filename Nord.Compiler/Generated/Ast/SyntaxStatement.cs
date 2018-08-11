using System;
using System.Collections.Generic;
using LanguageExt;

namespace Nord.Compiler.Generated.Ast
{
    public class SyntaxStatement
    {
        public int Position
        {
            get;
            private set;
        }

        public virtual SyntaxStatement Copy()
        {
            var copy = (SyntaxStatement)(new SyntaxStatement());
            copy.Position = this.Position;
            return copy;
        }

        SyntaxStatement WithPosition(int value)
        {
            var copy = (SyntaxStatement)(this.Copy());
            copy.Position = value;
            return copy;
        }
    }
}