using System;

namespace Nord.Compiler.Syntax
{
    [Serializable]
    public enum SyntaxModifier
    {
        None = 0,
        Public,
        Private,
        Open,
        Final,
        Abstract
    }
}