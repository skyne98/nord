using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using Nord.Compiler.Ast;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.StatementLets;
using Nord.Compiler.Generated.Ast.Statements;
using Nord.Compiler.Generated.Ast.Types;
using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class StatementParser
    {
        // Modifiers
        public static TokenListParser<TokenType, SyntaxModifier> VisibilityModifier { get; } =
            from keyword in Token.EqualTo(TokenType.PublicKeyword)
                .Or(Token.EqualTo(TokenType.PrivateKeyword))
            select keyword.ToStringValue() == "pub" 
                ? SyntaxModifier.Public 
                : SyntaxModifier.Private;

        public static TokenListParser<TokenType, SyntaxModifier> OpenModifier { get; } =
            from open in Token.EqualTo(TokenType.OpenKeyword)
            select SyntaxModifier.Open;
        
        public static TokenListParser<TokenType, SyntaxModifier> AbstractModifier { get; } =
            from abs in Token.EqualTo(TokenType.AbstractKeyword)
            select SyntaxModifier.Abstract;

        public static TokenListParser<TokenType, SyntaxModifier> FinalModifier { get; } =
            from final in Token.EqualTo(TokenType.FinalKeyword)
            select SyntaxModifier.Final;

        public static TokenListParser<TokenType, SyntaxModifier[]> Modifiers { get; } =
            from visibility in VisibilityModifier.Optional()
            from open in OpenModifier.Optional()
            from abs in AbstractModifier.Optional()
            from final in FinalModifier.Optional()
            select (new Func<SyntaxModifier[]>(() => {
                var list = new List<SyntaxModifier>();
                if (visibility != null)
                    list.Add(visibility.Value);
                else
                    list.Add(SyntaxModifier.Private);
                if (open != null)
                    list.Add(open.Value);
                if (abs != null)
                    list.Add(abs.Value);
                if (final != null)
                    list.Add(final.Value);

                return list.ToArray();
            }))();
        
        // Statements
        public static TokenListParser<TokenType, SyntaxStatementExpression> ExpressionStatement { get; } =
            from expression in ExpressionParser.Expression
            select new SyntaxStatementExpression()
                .WithExpression(expression);

        public static TokenListParser<TokenType, SyntaxStatementFunction> FnStatement { get; } =
            from fn in Token.EqualTo(TokenType.FnKeyword)
            from name in Token.EqualTo(TokenType.Identifier)
            from typeParameters in TypeParser.TypeParameters.OptionalOrDefault()
            from openParen in Token.EqualTo(TokenType.OpenParen)
            from parameters in TypeParser.Declarator.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
            from closeParen in Token.EqualTo(TokenType.CloseParen)
            from returnType in (
                    from colon in Token.EqualTo(TokenType.Colon)
                    from returnAnnotation in TypeParser.TypeReference
                    select returnAnnotation)
                .OptionalOrDefault()
                .Select(an => an != null ? Option<SyntaxTypeReference>.Some(an) : Option<SyntaxTypeReference>.None)
            from openCurly in Token.EqualTo(TokenType.OpenCurly)
            from statements in Parsers.StatementsBlock
            from closeCurly in Token.EqualTo(TokenType.CloseCurly)
            select new SyntaxStatementFunction()
                .WithName(name.ToStringValue())
                .WithTypeParameters(typeParameters ?? new SyntaxTypeParameter[] { })
                .WithParameters(parameters)
                .WithReturn(returnType)
                .WithBody(statements);

        public static TokenListParser<TokenType, SyntaxStatementLet> LetStatement { get; } =
            from letKeyword in Token.EqualTo(TokenType.LetKeyword)
            from declarator in TypeParser.Declarator
                .Select(dec => (Either<SyntaxTypeDeclarator, SyntaxDestructuringPattern>) dec)
                .Or(Parsers.DestructuringPattern
                    .Select(dp => (Either<SyntaxTypeDeclarator, SyntaxDestructuringPattern>) dp))
            from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
            from value in ExpressionParser.Expression
            select declarator.Match(
                right =>
                    new SyntaxStatementLetDestructuring()
                        .WithPattern(right)
                        .WithExpression(value),
                left => 
                    new SyntaxStatementLetDeclarator()
                        .WithDeclarator(left)
                        .WithExpression(value)
                );

        public static TokenListParser<TokenType, SyntaxStatementReturn> ReturnStatement { get; } =
            from returnKeywork in Token.EqualTo(TokenType.ReturnKeyword)
            from value in ExpressionParser.Expression
            select new SyntaxStatementReturn()
                .WithExpression(value);

        public static TokenListParser<TokenType, SyntaxStatementBreak> BreakStatement { get; } =
            from breakKeywork in Token.EqualTo(TokenType.BreakKeyword)
            select new SyntaxStatementBreak();

        public static TokenListParser<TokenType, SyntaxStatementContinue> ContinueStatement { get; } =
            from continueKeyword in Token.EqualTo(TokenType.ContinueKeyword)
            select new SyntaxStatementContinue();

        public static TokenListParser<TokenType, SyntaxStatementLoop> LoopStatement { get; } =
            from loopKeyword in Token.EqualTo(TokenType.LoopKeyword)
            from openCurly in Token.EqualTo(TokenType.OpenCurly)
            from body in Parsers.StatementsBlock
            from closeCurly in Token.EqualTo(TokenType.CloseCurly)
            select new SyntaxStatementLoop()
                .WithBody(body);
        
        public static TokenListParser<TokenType, SyntaxStatement> Statement { get; } =
            LoopStatement.Select(s => (SyntaxStatement) s)
                .Or(LetStatement.Select(l => (SyntaxStatement) l))
                .Or(FnStatement.Select(fn => (SyntaxStatement) fn))
                .Or(ReturnStatement.Select(r => (SyntaxStatement) r))
                .Or(BreakStatement.Select(b => (SyntaxStatement) b))
                .Or(ContinueStatement.Select(c => (SyntaxStatement) c))
                .Or(ExpressionStatement.Select(e => (SyntaxStatement) e));
        
        // Top-level only statements
        public static TokenListParser<TokenType, SyntaxStatementClassDeclaration> ClassStatement { get; } =
            from classKeyword in Token.EqualTo(TokenType.ClassKeyword)
            from name in Token.EqualTo(TokenType.Identifier)
            from openCurly in Token.EqualTo(TokenType.OpenCurly)
            from body in Parsers.TopLevelStatementBlock
            from closeCurly in Token.EqualTo(TokenType.CloseCurly)
            select new SyntaxStatementClassDeclaration()
                .WithName(name.ToStringValue())
                .WithBody(body);

        public static TokenListParser<TokenType, SyntaxStatementUse> UseStatement { get; } =
            from useKeyword in Token.EqualTo(TokenType.UseKeyword)
            from alias in Token.EqualTo(TokenType.Identifier)
                .Select(alias => (Either<string, SyntaxDestructuringPattern>) alias.ToStringValue())
                .Or(Parsers.DestructuringPattern
                    .Select(dp => (Either<string, SyntaxDestructuringPattern>) dp))
            from @from in Token.EqualTo(TokenType.FromKeyword)
            from path in Token.EqualTo(TokenType.String)
            select new SyntaxStatementUse()
                .WithAlias(alias)
                .WithFrom(path.ToStringValue());
        
        public static TokenListParser<TokenType, SyntaxStatementTopLevel> TopLevelStatement { get; } =
            from modifiers in Modifiers
            from statement in FnStatement.Select(fn => (SyntaxStatement) fn)
                .Or(ClassStatement.Select(c => (SyntaxStatement) c))
                .Or(UseStatement.Select(u => (SyntaxStatement) u))
            select new SyntaxStatementTopLevel()
                .WithModifiers(modifiers)
                .WithStatement(statement);
    }
}
