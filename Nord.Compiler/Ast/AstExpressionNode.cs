using System;
using System.Collections.Generic;
using System.Text;

namespace Nord.Compiler.Ast
{
    public class AstExpressionNode: AstNode
    {
        
    }

    public class AstExpressionLiteralNode<V> : AstExpressionNode
    {
        public AstExpressionLiteralNode(V value)
        {
            Value = value;
        }

        public V Value { get; private set; }
    }
}
