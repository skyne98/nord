using System;

namespace Nord.Compiler.Ast
{
    [Serializable]
    public enum AstModifier
    {
        None = 0,
        Public,
        Private,
        Open,
        Final,
        Abstract
    }
}