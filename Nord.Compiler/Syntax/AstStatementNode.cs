using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.ClassInstances;
using Nord.Compiler.Syntax;

namespace Nord.Compiler.Ast
{
    [Serializable]
    public class AstStatementNode: AstNode
    {

    }

    [Serializable]
    public class AstStatementExpressionNode : AstStatementNode
    {
        public AstStatementExpressionNode(AstExpressionNode expression)
        {
            Expression = expression;
        }

        public AstExpressionNode Expression { get; private set; }
    }

    [Serializable]
    public class AstStatementFunctionNode : AstStatementNode
    {
        public AstStatementFunctionNode(string name, AstTypeParameterNode[] typeParameters, AstTypeDeclaratorNode[] parameters, Option<AstTypeReferenceNode> @return, AstStatementNode[] body)
        {
            Name = name;
            TypeParameters = typeParameters;
            Parameters = parameters;
            Return = @return;
            Body = body;
        }

        public string Name { get; private set; }
        public AstTypeParameterNode[] TypeParameters { get; private set; }
        public AstTypeDeclaratorNode[] Parameters { get; private set; }
        public Option<AstTypeReferenceNode> Return { get; private set; }
        public AstStatementNode[] Body { get; private set; }
    }

    [Serializable]
    public class AstStatementReturnNode : AstStatementNode
    {
        public AstStatementReturnNode(AstExpressionNode value)
        {
            Value = value;
        }

        public AstExpressionNode Value { get; private set; }
    }

    [Serializable]
    public class AstStatementBreakNode : AstStatementNode
    {

    }

    [Serializable]
    public class AstStatementContinueNode : AstStatementNode
    {

    }

    // Loops
    [Serializable]
    public class AstStatementLoopNode: AstStatementNode
    {
        public AstStatementLoopNode(AstStatementNode[] statements)
        {
            Statements = statements;
        }

        public AstStatementNode[] Statements { get; private set; }
    }
    
    // Declarations
    [Serializable]
    public class AstStatementLetNode : AstStatementNode
    {
        public AstStatementLetNode(Either<AstTypeDeclaratorNode, AstDestructuringPatternNode> declarator, AstExpressionNode value)
        {
            Declarator = declarator;
            Value = value;
        }

        public Either<AstTypeDeclaratorNode, AstDestructuringPatternNode> Declarator { get; private set; }
        public AstExpressionNode Value { get; private set; }
    }

    [Serializable]
    public class AstStatementClassNode : AstStatementNode
    {
        public AstStatementClassNode(string name, AstStatementTopLevelNode[] body)
        {
            Name = name;
            Body = body;
        }

        public string Name { get; private set; }
        public AstStatementTopLevelNode[] Body { get; private set; }
    }

    [Serializable]
    public class AstStatementUseNode : AstStatementNode
    {
        public AstStatementUseNode(Either<string, AstDestructuringPatternNode> @alias, string @from)
        {
            Alias = alias;
            From = @from;
        }

        public Either<string, AstDestructuringPatternNode> Alias { get; private set; }
        public string From { get; private set; }
    }

    [Serializable]
    public class AstStatementTopLevelNode : AstStatementNode
    {
        public AstStatementTopLevelNode(SyntaxModifier[] modifiers, AstStatementNode statement)
        {
            Modifiers = modifiers;
            Statement = statement;
        }
        
        public SyntaxModifier[] Modifiers { get; private set; }
        public AstStatementNode Statement { get; private set; }
    }
}