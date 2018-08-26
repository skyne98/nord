using System;
using System.Collections.Generic;
using LanguageExt;
using Nord.Compiler.Generated.Ast.Nodes;

namespace Nord.Compiler.Generated.Ast.DestructuringPatterns
{
    public class SyntaxDestructuringArrayPattern : SyntaxDestructuringPattern
    {
        public Either<string, SyntaxDestructuringPattern>[] Aliases
        {
            get;
            private set;
        }

        public Option<string> Rest
        {
            get;
            private set;
        }

        public override SyntaxNode Copy()
        {
            var copy = (SyntaxDestructuringArrayPattern)(base.Copy());
            copy.Aliases = this.Aliases;
            copy.Rest = this.Rest;
            return copy;
        }

        SyntaxDestructuringArrayPattern WithAliases(Either<string, SyntaxDestructuringPattern>[] value)
        {
            var copy = (SyntaxDestructuringArrayPattern)(this.Copy());
            copy.Aliases = value;
            return copy;
        }

        SyntaxDestructuringArrayPattern WithRest(Option<string> value)
        {
            var copy = (SyntaxDestructuringArrayPattern)(this.Copy());
            copy.Rest = value;
            return copy;
        }
    }
}