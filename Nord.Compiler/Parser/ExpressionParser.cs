using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using LanguageExt;
using Nord.Compiler.Ast;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class ExpressionParser
    {
        public static TokenListParser<TokenType, AstExpressionNode> Literal { get; } =
            LiteralParser.String.Select(e => (AstExpressionNode) e)
                .Or(LiteralParser.Double.Select(e => (AstExpressionNode) e))
                .Or(LiteralParser.Identifier.Select(e => (AstExpressionNode) e));

        public static TokenListParser<TokenType, AstExpressionIfNode> If { get; } =
            from ifKeyword in Token.EqualTo(TokenType.IfKeyword)
            from conditionExpression in ExpressionParser.Expression
            from thenExpression in ExpressionParser.Expression
            from elseKeyword in Token.EqualTo(TokenType.ElseKeyword)
            from elseExpression in ExpressionParser.Expression
            select new AstExpressionIfNode(conditionExpression, thenExpression, elseExpression);

        // 0 precedence
        public static TokenListParser<TokenType, AstExpressionNode> Terminal { get; } =
            (
                from openParen in Token.EqualTo(TokenType.OpenParen)
                from expression in Parse.Ref(() => Expression)
                from closeParen in Token.EqualTo(TokenType.CloseParen)
                select expression
            )
            .Or(Literal);
        
        // 1 precedence
        public static Func<AstExpressionNode, TokenListParser<TokenType, AstExpressionCallNode>> CallTail { get; } =
            s =>    
                from openParen in Token.EqualTo(TokenType.OpenParen)
                from arguments in (
                    from expressions in Parse.Ref(() => Expression).ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
                    select expressions
                )
                from closeParen in Token.EqualTo(TokenType.CloseParen)
                select new AstExpressionCallNode(s, arguments);

        public static Func<AstExpressionNode, TokenListParser<TokenType, AstExpressionMemberNode>> MemberTail { get; } =
            s =>
                from openSquare in Token.EqualTo(TokenType.OpenSquare)
                from index in Parse.Ref(() => Expression)
                from closeSquare in Token.EqualTo(TokenType.CloseSquare)
                select new AstExpressionMemberNode(s, index);

        public static Func<AstExpressionNode, TokenListParser<TokenType, AstExpressionPropertyAccessNode>> PropertyAccessTail { get; } =
            s =>
                from dot in Token.EqualTo(TokenType.DotOperator)
                from property in Token.EqualTo(TokenType.Identifier)
                select new AstExpressionPropertyAccessNode(s, property.ToStringValue());

        public static Func<AstExpressionNode, TokenListParser<TokenType, AstExpressionNode>> PostfixTail { get; } =
            s =>
                CallTail(s).Select(c => (AstExpressionNode) c)
                    .Or(MemberTail(s).Select(m => (AstExpressionNode) m))
                    .Or(PropertyAccessTail(s).Select(p => (AstExpressionNode) p));

        public static TokenListParser<TokenType, AstExpressionNode> Postfix { get; } =
            Parsers.RightRec(Terminal, PostfixTail);
        
        // 2 precedence
        public static TokenListParser<TokenType, AstExpressionNode> Unary { get; } =
            (from op in OperatorParser.BangOperator
                    .Or(OperatorParser.MinusOperator)
                from unary in Parse.Ref(() => Unary)
                select new AstExpressionUnaryNode(op, unary))
            .Select(u => (AstExpressionNode) u)
            .Or(Postfix);
        
        // 3 precedence
        public static TokenListParser<TokenType, AstExpressionNode> Multiplication { get; } =
            Parse.Chain(OperatorParser.MultiplyOperator.Or(OperatorParser.DivideOperator), Unary, (op, left, right) =>
                new AstExpressionBinaryNode(op, left, right)    
            );
        
        // 4 precedence
        public static TokenListParser<TokenType, AstExpressionNode> Addition { get; } =
            Parse.Chain(OperatorParser.PlusOperator.Or(OperatorParser.MinusOperator), Multiplication, (op, left, right) =>
                new AstExpressionBinaryNode(op, left, right)
            );

        // 5 precedence
        public static Func<AstExpressionNode, TokenListParser<TokenType, AstExpressionNode>> AsTail { get; } =
            s => 
                from asKeyword in OperatorParser.CastOperator
                from type in TypeParser.TypeAnnotation
                select (AstExpressionNode)new AstExpressionAsNode(s, type);


        public static TokenListParser<TokenType, AstExpressionNode> As { get; } =
            Parsers.RightRec(Addition, AsTail);

        public static TokenListParser<TokenType, AstExpressionNode> Comparison { get; } =
            Parse.Chain(OperatorParser.LessThanOperator
                    .Or(OperatorParser.LessThanEqualsOperator)
                    .Or(OperatorParser.MoreThanOperator)
                    .Or(OperatorParser.MoreThanEqualsOperator), As, (op, left, right) => 
                    new AstExpressionBinaryNode(op, left, right)
            );
        
        // 7 precedence
        public static TokenListParser<TokenType, AstExpressionNode> Equality { get; } =
            Parse.Chain(OperatorParser.EqualsOperator
                    .Or(OperatorParser.NotEqualsOperator), Comparison, (op, left, right) =>
                    new AstExpressionBinaryNode(op, left, right)
            );
            
        public static TokenListParser<TokenType, AstExpressionNode> Expression { get; } =
            If.Select(i => (AstExpressionNode)i)
                .Or(Equality);
    }
}
