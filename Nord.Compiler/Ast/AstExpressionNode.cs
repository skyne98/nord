using System;
using System.Collections.Generic;
using System.Text;
using Nord.Compiler.Lexer;

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

    public class AstExpressionLetNode : AstExpressionNode
    {
        public AstExpressionLetNode(AstTypeDeclaratorNode declarator, AstExpressionNode value)
        {
            Declarator = declarator;
            Value = value;
        }

        public AstTypeDeclaratorNode Declarator { get; private set; }
        public AstExpressionNode Value { get; private set; }
    }

    public class AstExpressionBinaryNode : AstExpressionNode
    {
        public AstExpressionBinaryNode(TokenType @operator, AstExpressionNode left, AstExpressionNode right)
        {
            Operator = @operator;
            Left = left;
            Right = right;
        }

        public TokenType Operator { get; private set; }
        public AstExpressionNode Left { get; private set; }
        public AstExpressionNode Right { get; private set; }
    }

    public class AstExpressionUnaryNode : AstExpressionNode
    {
        public AstExpressionUnaryNode(TokenType @operator, AstExpressionNode value)
        {
            Operator = @operator;
            Value = value;
        }

        public TokenType Operator { get; private set; }
        public AstExpressionNode Value { get; private set; }
    }
}
