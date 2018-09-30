using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using LanguageExt;
using Nord.Compiler.Generated.Ast.ExpressionPostfixes;
using Nord.Compiler.Generated.Ast.Expressions;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Types;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class ExpressionParser
    {
        public static TokenListParser<TokenType, SyntaxExpression> Literal { get; } =
            LiteralParser.String.Select(e => (SyntaxExpression) e)
                .Or(LiteralParser.Double.Select(e => (SyntaxExpression) e))
                .Or(LiteralParser.Identifier.Select(e => (SyntaxExpression) e));

        public static TokenListParser<TokenType, SyntaxExpressionIf> If { get; } =
            from ifKeyword in Token.EqualTo(TokenType.IfKeyword)
            from conditionExpression in ExpressionParser.Expression
            from thenExpression in ExpressionParser.Expression
            from elseKeyword in Token.EqualTo(TokenType.ElseKeyword)
            from elseExpression in ExpressionParser.Expression
            select new SyntaxExpressionIf()
                .WithCondition(conditionExpression)
                .WithThen(thenExpression)
                .WithElse(elseExpression);

        // 0 precedence
        public static TokenListParser<TokenType, SyntaxExpression> Terminal { get; } =
            (
                from openParen in Token.EqualTo(TokenType.OpenParen)
                from expression in Parse.Ref(() => Expression)
                from closeParen in Token.EqualTo(TokenType.CloseParen)
                select expression
            )
            .Or(Literal);
        
        // 1 precedence
        public static Func<SyntaxExpression, TokenListParser<TokenType, SyntaxExpressionCall>> CallTail { get; } =
            s =>    
                from typeParameters in TypeParser.TypeArguments.OptionalOrDefault()
                from openParen in Token.EqualTo(TokenType.OpenParen)
                from arguments in (
                    from expressions in Parse.Ref(() => Expression).ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
                    select expressions
                )
                from closeParen in Token.EqualTo(TokenType.CloseParen)
                select new SyntaxExpressionCall()
                    .WithCallee(s)
                    .WithTypeParameters(typeParameters);

        public static Func<SyntaxExpression, TokenListParser<TokenType, SyntaxExpressionMember>> MemberTail { get; } =
            s =>
                from openSquare in Token.EqualTo(TokenType.OpenSquare)
                from index in Parse.Ref(() => Expression)
                from closeSquare in Token.EqualTo(TokenType.CloseSquare)
                select new SyntaxExpressionMember()
                    .WithCallee(s)
                    .WithIndex(index);

        public static Func<SyntaxExpression, TokenListParser<TokenType, SyntaxExpressionProperty>> PropertyAccessTail { get; } =
            s =>
                from dot in Token.EqualTo(TokenType.DotOperator)
                from property in Token.EqualTo(TokenType.Identifier)
                select new SyntaxExpressionProperty()
                    .WithLeft(s)
                    .WithName(property.ToStringValue());

        public static Func<SyntaxExpression, TokenListParser<TokenType, SyntaxExpression>> PostfixTail { get; } =
            s =>
                CallTail(s).Select(c => (SyntaxExpression) c)
                    .Or(MemberTail(s).Select(m => (SyntaxExpression) m))
                    .Or(PropertyAccessTail(s).Select(p => (SyntaxExpression) p));

        public static TokenListParser<TokenType, SyntaxExpression> Postfix { get; } =
            Parsers.RightRec(Terminal, PostfixTail);
        
        // 2 precedence
        public static TokenListParser<TokenType, SyntaxExpression> Unary { get; } =
            (from op in OperatorParser.BangOperator
                    .Or(OperatorParser.MinusOperator)
                from unary in Parse.Ref(() => Unary)
                select new SyntaxExpressionUnary()
                    .WithOperator(op)
                    .WithValue(unary))
            .Select(u => (SyntaxExpression) u)
            .Or(Postfix);
        
        // 3 precedence
        public static TokenListParser<TokenType, SyntaxExpression> Multiplication { get; } =
            Parse.Chain(OperatorParser.MultiplyOperator.Or(OperatorParser.DivideOperator), Unary, (op, left, right) =>
                new SyntaxExpressionBinary()
                    .WithOperator(op)
                    .WithLeft(left)
                    .WithRight(right)
            );
        
        // 4 precedence
        public static TokenListParser<TokenType, SyntaxExpression> Addition { get; } =
            Parse.Chain(OperatorParser.PlusOperator.Or(OperatorParser.MinusOperator), Multiplication, (op, left, right) =>
                new SyntaxExpressionBinary()
                    .WithOperator(op)
                    .WithLeft(left)
                    .WithRight(right)
            );

        // 5 precedence
        public static Func<SyntaxExpression, TokenListParser<TokenType, SyntaxExpression>> AsTail { get; } =
            s => 
                from asKeyword in OperatorParser.CastOperator
                from type in TypeParser.TypeReference
                select (SyntaxExpression)new SyntaxExpressionAs()
                    .WithType(type)
                    .WithExpression(s);


        public static TokenListParser<TokenType, SyntaxExpression> As { get; } =
            Parsers.RightRec(Addition, AsTail);

        public static TokenListParser<TokenType, SyntaxExpression> Comparison { get; } =
            Parse.Chain(OperatorParser.LessThanOperator
                    .Or(OperatorParser.LessThanEqualsOperator)
                    .Or(OperatorParser.MoreThanOperator)
                    .Or(OperatorParser.MoreThanEqualsOperator), As, (op, left, right) => 
                    new SyntaxExpressionBinary()
                        .WithOperator(op)
                        .WithLeft(left)
                        .WithRight(right)
            );
        
        // 7 precedence
        public static TokenListParser<TokenType, SyntaxExpression> Equality { get; } =
            Parse.Chain(OperatorParser.EqualsOperator
                    .Or(OperatorParser.NotEqualsOperator), Comparison, (op, left, right) =>
                    new SyntaxExpressionBinary()
                        .WithOperator(op)
                        .WithLeft(left)
                        .WithRight(right)
            );

        // 8 precedence
        public static TokenListParser<TokenType, SyntaxExpression> Assignment { get; } =
            Parse.ChainRight(OperatorParser.AssignmentOperator, 
                Equality.Try().Or(Comparison), 
                (op, left, right) => new SyntaxExpressionBinary()
                    .WithOperator(op)
                    .WithLeft(left)
                    .WithRight(right));

        public static TokenListParser<TokenType, SyntaxExpression> Expression { get; } =
            If.Select(i => (SyntaxExpression)i)
                .Or(Assignment);
    }
}
