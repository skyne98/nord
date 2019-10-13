using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using LanguageExt;
using Nord.Compiler.Ast;
using Nord.Compiler.Generated.Ast.DestructuringPatterns;
using Nord.Compiler.Generated.Ast.Nodes;
using Nord.Compiler.Generated.Ast.Statements;
using Nord.Compiler.Lexer;
using Nord.Compiler.Parser;
using Superpower;
using Superpower.Util;
using Superpower.Model;
using Superpower.Parsers;

public static class Parsers
{
    public static TokenListParser<TokenType, SyntaxStatement[]> StatementsBlock { get; } =
        from statements in Parse.Ref(() => StatementParser.Statement)
            .ManyDelimitedBy(Token.EqualTo(TokenType.Semicolon).OptionalOrDefault())
        select statements;

    public static TokenListParser<TokenType, SyntaxStatementTopLevel[]> TopLevelStatementBlock { get; } = 
        from statements in Parse.Ref(() => StatementParser.TopLevelStatement)
            .ManyDelimitedBy(Token.EqualTo(TokenType.Semicolon).OptionalOrDefault())
        select statements;

    // Destructuring -> Array
    public static TokenListParser<TokenType, Either<string, SyntaxDestructuringPattern>> DestructuringPatternOrIdentifier
    {
        get;
    } =
        Token.EqualTo(TokenType.Identifier)
            .Select(t => (Either<string, SyntaxDestructuringPattern>) t.ToStringValue())
            .Or(Parse.Ref(() => DestructuringPattern)
                .Select(p => (Either<string, SyntaxDestructuringPattern>) p));
    
    public static TokenListParser<TokenType, SyntaxDestructuringArrayPattern> DestructuringArrayElements { get; } =
        from names in DestructuringPatternOrIdentifier.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        select new SyntaxDestructuringArrayPattern()
            .WithAliases(names)
            .WithRest(null);
    
    //TODO: Seems to be not working properly, three dots are not parsed
    public static TokenListParser<TokenType, SyntaxDestructuringArrayPattern> DestructuringArrayElementsWithRest { get; } =
        from names in DestructuringPatternOrIdentifier.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        from comma in Token.EqualTo(TokenType.Comma)
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new SyntaxDestructuringArrayPattern()
            .WithAliases(names)
            .WithRest(rest.ToStringValue());
    
    public static TokenListParser<TokenType, SyntaxDestructuringArrayPattern> DestructuringArrayElementsRestOnly { get; } =
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new SyntaxDestructuringArrayPattern()
            .WithAliases(new Either<string, SyntaxDestructuringPattern>[]{})
            .WithRest(rest.ToStringValue());

    public static TokenListParser<TokenType, SyntaxDestructuringArrayPattern> DestructuringArrayPattern { get; } =
        from openSquare in Token.EqualTo(TokenType.OpenSquare)
        from elements in DestructuringArrayElements
            .Or(DestructuringArrayElementsWithRest)
            .Or(DestructuringArrayElementsRestOnly)
        from closeSquare in Token.EqualTo(TokenType.CloseSquare)
        select elements;
    
    // Destructuring -> Tuple
    public static TokenListParser<TokenType, SyntaxDestructuringTuplePatternNode> DestructuringTupleElements { get; } =
        from names in DestructuringPatternOrIdentifier.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        select new SyntaxDestructuringTuplePatternNode()
            .WithAliases(names)
            .WithRest(null);

    public static TokenListParser<TokenType, SyntaxDestructuringTuplePatternNode> DestructuringTupleElementsWithRest
    {
        get;
    } =
        from names in DestructuringPatternOrIdentifier.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        from comma in Token.EqualTo(TokenType.Comma)
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new SyntaxDestructuringTuplePatternNode()
            .WithAliases(names)
            .WithRest(rest.ToStringValue());
    
    public static TokenListParser<TokenType, SyntaxDestructuringTuplePatternNode> DestructuringTupleElementsRestOnly { get; } =
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new SyntaxDestructuringTuplePatternNode()
            .WithAliases(new Either<string, SyntaxDestructuringPattern>[]{})
            .WithRest(rest.ToStringValue());

    public static TokenListParser<TokenType, SyntaxDestructuringTuplePatternNode> DestructuringTuplePattern { get; } =
        from openParen in Token.EqualTo(TokenType.OpenParen)
        from elements in DestructuringTupleElements
            .Or(DestructuringTupleElementsWithRest)
            .Or(DestructuringTupleElementsRestOnly)
        from closeParen in Token.EqualTo(TokenType.CloseParen)
        select elements;
    
    // Destructuring -> Object
    public static TokenListParser<TokenType, Either<string, SyntaxDestructuringPattern>> DestructuringBindingAlias { get; } =
        from asKeyword in OperatorParser.CastOperator
        from alias in DestructuringPatternOrIdentifier
        select alias;

    public static TokenListParser<TokenType, SyntaxDestructuringPatternBindingElement> DestructuringBindingElement { get; }
        =
        from name in Token.EqualTo(TokenType.Identifier)
        from alias in DestructuringBindingAlias.OptionalOrDefault()
        select new SyntaxDestructuringPatternBindingElement()
            .WithProperty(name.ToStringValue())
            .WithAlias(alias);
    
    public static TokenListParser<TokenType, SyntaxDestructuringObjectPattern> DestructuringObjectElements { get; } =
        from names in DestructuringBindingElement.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        select new SyntaxDestructuringObjectPattern()
            .WithBindingElements(names)
            .WithRest(null);
    
    public static TokenListParser<TokenType, SyntaxDestructuringObjectPattern> DestructuringObjectElementsWithRest { get; } =
        from names in DestructuringBindingElement.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        from comma in Token.EqualTo(TokenType.Comma)
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new SyntaxDestructuringObjectPattern()
            .WithBindingElements(names.ToArray())
            .WithRest(rest.ToStringValue());
    
    public static TokenListParser<TokenType, SyntaxDestructuringObjectPattern> DestructuringObjectElementsRestOnly { get; } =
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new SyntaxDestructuringObjectPattern()
            .WithBindingElements(new SyntaxDestructuringPatternBindingElement[] {})
            .WithRest(rest.ToStringValue());

    public static TokenListParser<TokenType, SyntaxDestructuringObjectPattern> DestructuringObjectPattern { get; } =
        from openCurly in Token.EqualTo(TokenType.OpenCurly)
        from elements in DestructuringObjectElements
            .Or(DestructuringObjectElementsWithRest)
            .Or(DestructuringObjectElementsRestOnly)
        from closeCurly in Token.EqualTo(TokenType.CloseCurly)
        select elements;

    public static TokenListParser<TokenType, SyntaxDestructuringPattern> DestructuringPattern { get; } =
        DestructuringArrayPattern.Select(ap => (SyntaxDestructuringPattern) ap)
            .Or(DestructuringTuplePattern.Select(tp => (SyntaxDestructuringPattern) tp))
            .Or(DestructuringObjectPattern.Select(op => (SyntaxDestructuringPattern) op));

    public static TokenListParser<TokenType, SyntaxDocument> Document { get; } =
        from statements in Parsers.TopLevelStatementBlock
        select new SyntaxDocument().WithStatements(statements);
    
    public static TokenListParser<TKind, T> RightRec<TKind, T>(TokenListParser<TKind, T> head, Func<T, TokenListParser<TKind, T>> apply)
    {
        if (head == null) throw new ArgumentNullException(nameof(head));
        if (apply == null) throw new ArgumentNullException(nameof(apply));

        return input =>
        {
            var parseResult = head(input);
            if (!parseResult.HasValue)
                return parseResult;

            var result = parseResult.Value;

            var tailResult = apply(result)(parseResult.Remainder);
            while (true)
            {
                if (!tailResult.HasValue) 
                    return TokenListParserResult.Value(result, input, tailResult.Remainder);
                
                result = tailResult.Value;
                tailResult = apply(result)(tailResult.Remainder);
            }
        };
    }

    public static bool TryParse(string code, out object value, out string error, out Position errorPosition)
    {
        var tokenizer = new NordTokenizer();
        var tokens = tokenizer.TryTokenize(code);
        if (!tokens.HasValue)
        {
            value = null;
            error = tokens.ToString();
            errorPosition = tokens.ErrorPosition;
            return false;
        }

        var parsed = Parsers.Document.TryParse(tokens.Value);
        if (!parsed.HasValue)
        {
            value = null;
            error = parsed.ToString();
            errorPosition = parsed.ErrorPosition;
            return false;
        }

        value = parsed.Value;
        error = null;
        errorPosition = Position.Empty;
        return true;
    }
}