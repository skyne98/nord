using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using Nord.Compiler.Ast;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class StatementParser
    {
        public static TokenListParser<TokenType, AstStatementExpressionNode> ExpressionStatement { get; } =
            from expression in ExpressionParser.Expression
            select new AstStatementExpressionNode(expression);

        public static TokenListParser<TokenType, AstStatementFunctionNode> FnStatement { get; } =
            from fn in Token.EqualTo(TokenType.FnKeyword)
            from name in Token.EqualTo(TokenType.Identifier)
            from typeParameters in TypeParser.TypeParameters
            from openParen in Token.EqualTo(TokenType.OpenParen)
            from parameters in TypeParser.Declarator.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
            from closeParen in Token.EqualTo(TokenType.CloseParen)
            from returnType in (
                    from colon in Token.EqualTo(TokenType.Colon)
                    from returnAnnotation in TypeParser.TypeReference
                    select returnAnnotation)
                .OptionalOrDefault()
                .Select(an => an != null ? Option<AstTypeReferenceNode>.Some(an) : Option<AstTypeReferenceNode>.None)
            from openCurly in Token.EqualTo(TokenType.OpenCurly)
            from statements in Parsers.StatementsBlock
            from closeCurly in Token.EqualTo(TokenType.CloseCurly)
            select new AstStatementFunctionNode(name.ToStringValue(), typeParameters, parameters, returnType, statements);

        public static TokenListParser<TokenType, AstStatementLetNode> LetStatement { get; } =
            from letKeyword in Token.EqualTo(TokenType.LetKeyword)
            from declarator in TypeParser.Declarator
            from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
            from value in ExpressionParser.Expression
            select new AstStatementLetNode(declarator, value);

        public static TokenListParser<TokenType, AstStatementReturnNode> ReturnStatement { get; } =
            from returnKeywork in Token.EqualTo(TokenType.ReturnKeyword)
            from value in ExpressionParser.Expression
            select new AstStatementReturnNode(value);

        public static TokenListParser<TokenType, AstStatementBreakNode> BreakStatement { get; } =
            from breakKeywork in Token.EqualTo(TokenType.BreakKeyword)
            select new AstStatementBreakNode();

        public static TokenListParser<TokenType, AstStatementContinueNode> ContinueStatement { get; } =
            from continueKeyword in Token.EqualTo(TokenType.ContinueKeyword)
            select new AstStatementContinueNode();

        public static TokenListParser<TokenType, AstStatementLoopNode> LoopStatement { get; } =
            from loopKeyword in Token.EqualTo(TokenType.LoopKeyword)
            from openCurly in Token.EqualTo(TokenType.OpenCurly)
            from body in Parsers.StatementsBlock
            from closeCurly in Token.EqualTo(TokenType.CloseCurly)
            select new AstStatementLoopNode(body);

        public static TokenListParser<TokenType, AstStatementNode> Statement { get; } =
            LoopStatement.Select(s => (AstStatementNode) s)
                .Or(LetStatement.Select(l => (AstStatementNode) l))
                .Or(FnStatement.Select(fn => (AstStatementNode) fn))
                .Or(ReturnStatement.Select(r => (AstStatementNode) r))
                .Or(BreakStatement.Select(b => (AstStatementNode) b))
                .Or(ContinueStatement.Select(c => (AstStatementNode) c))
                .Or(ExpressionStatement.Select(e => (AstStatementNode) e));
    }
}
