using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
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

    public class AstExpressionIdentifierLiteralNode : AstExpressionNode
    {
        public AstExpressionIdentifierLiteralNode(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }

    public class AstExpressionIfNode : AstExpressionNode
    {
        public AstExpressionIfNode(AstExpressionNode condition, AstExpressionNode then, AstExpressionNode @else)
        {
            Condition = condition;
            Then = then;
            Else = @else;
        }

        public AstExpressionNode Condition { get; private set; }
        public AstExpressionNode Then { get; private set; }
        public AstExpressionNode Else { get; private set; }
    }

    public class AstExpressionBinaryNode : AstExpressionNode
    {
        public AstExpressionBinaryNode(string @operator, AstExpressionNode left, AstExpressionNode right)
        {
            Operator = @operator;
            Left = left;
            Right = right;
        }

        public string Operator { get; private set; }
        public AstExpressionNode Left { get; private set; }
        public AstExpressionNode Right { get; private set; }
    }

    public class AstExpressionUnaryNode : AstExpressionNode
    {
        public AstExpressionUnaryNode(string @operator, AstExpressionNode value)
        {
            Operator = @operator;
            Value = value;
        }

        public string Operator { get; private set; }
        public AstExpressionNode Value { get; private set; }
    }

    public class AstPostfixNode : AstExpressionNode
    {
        
    }
    
    public class AstExpressionCallNode : AstPostfixNode
    {
        public AstExpressionCallNode(AstExpressionNode function, AstTypeAnnotationNode[] typeParameters, AstExpressionNode[] parameters)
        {
            Function = function;
            TypeParameters = typeParameters;
            Parameters = parameters;
        }

        public AstExpressionNode Function { get; private set; }
        public AstTypeAnnotationNode[] TypeParameters { get; private set; }

        public AstExpressionNode[] Parameters { get; private set; }
    }

    public class AstExpressionMemberNode : AstPostfixNode
    {
        public AstExpressionMemberNode(AstExpressionNode subject, AstExpressionNode index)
        {
            Subject = subject;
            Index = index;
        }

        public AstExpressionNode Subject { get; private set; }
        public AstExpressionNode Index { get; private set; }
    }

    public class AstExpressionPropertyAccessNode : AstPostfixNode
    {
        public AstExpressionPropertyAccessNode(AstExpressionNode left, string right)
        {
            Left = left;
            Right = right;
        }

        public AstExpressionNode Left { get; private set; }
        public string Right { get; private set; }
    }

    public class AstExpressionAsNode : AstExpressionNode
    {
        public AstExpressionAsNode(AstExpressionNode expression, AstTypeAnnotationNode type)
        {
            Expression = expression;
            Type = type;
        }

        public AstExpressionNode Expression { get; private set; }
        public AstTypeAnnotationNode Type { get; private set; }
    }
}
