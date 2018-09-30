using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using LanguageExt;
using Nord.Compiler.Ast;
using Nord.Compiler.Generated.Ast.Nodes;
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

    public static TokenListParser<TokenType, AstStatementTopLevelNode[]> TopLevelStatementBlock { get; } = 
        from statements in Parse.Ref(() => StatementParser.TopLevelStatement)
            .ManyDelimitedBy(Token.EqualTo(TokenType.Semicolon).OptionalOrDefault())
        select statements;

    // Destructuring -> Array
    public static TokenListParser<TokenType, Either<string, AstDestructuringPatternNode>> DestructuringPatternOrIdentifier
    {
        get;
    } =
        Token.EqualTo(TokenType.Identifier)
            .Select(t => (Either<string, AstDestructuringPatternNode>) t.ToStringValue())
            .Or(Parse.Ref(() => DestructuringPattern)
                .Select(p => (Either<string, AstDestructuringPatternNode>) p));
    
    public static TokenListParser<TokenType, AstDestructuringArrayPatternNode> DestructuringArrayElements { get; } =
        from names in DestructuringPatternOrIdentifier.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        select new AstDestructuringArrayPatternNode(names.ToArray(), null);
    
    public static TokenListParser<TokenType, AstDestructuringArrayPatternNode> DestructuringArrayElementsWithRest { get; } =
        from names in DestructuringPatternOrIdentifier.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        from comma in Token.EqualTo(TokenType.Comma)
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new AstDestructuringArrayPatternNode(names.ToArray(),
            rest.ToStringValue());
    
    public static TokenListParser<TokenType, AstDestructuringArrayPatternNode> DestructuringArrayElementsRestOnly { get; } =
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new AstDestructuringArrayPatternNode(new Either<string, AstDestructuringPatternNode>[]{}, rest.ToStringValue());

    public static TokenListParser<TokenType, AstDestructuringArrayPatternNode> DestructuringArrayPattern { get; } =
        from openSquare in Token.EqualTo(TokenType.OpenSquare)
        from elements in DestructuringArrayElements
            .Or(DestructuringArrayElementsWithRest)
            .Or(DestructuringArrayElementsRestOnly)
        from closeSquare in Token.EqualTo(TokenType.CloseSquare)
        select elements;
    
    // Destructuring -> Tuple
    public static TokenListParser<TokenType, AstDestructuringTuplePatternNode> DestructuringTupleElements { get; } =
        from names in DestructuringPatternOrIdentifier.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        select new AstDestructuringTuplePatternNode(names.ToArray(), null);
    
    public static TokenListParser<TokenType, AstDestructuringTuplePatternNode> DestructuringTupleElementsWithRest { get; } =
        from names in DestructuringPatternOrIdentifier.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        from comma in Token.EqualTo(TokenType.Comma)
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new AstDestructuringTuplePatternNode(names.ToArray(),
            rest.ToStringValue());
    
    public static TokenListParser<TokenType, AstDestructuringTuplePatternNode> DestructuringTupleElementsRestOnly { get; } =
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new AstDestructuringTuplePatternNode(new Either<string, AstDestructuringPatternNode>[]{}, rest.ToStringValue());

    public static TokenListParser<TokenType, AstDestructuringTuplePatternNode> DestructuringTuplePattern { get; } =
        from openParen in Token.EqualTo(TokenType.OpenParen)
        from elements in DestructuringTupleElements
            .Or(DestructuringTupleElementsWithRest)
            .Or(DestructuringTupleElementsRestOnly)
        from closeParen in Token.EqualTo(TokenType.CloseParen)
        select elements;
    
    // Destructuring -> Object
    public static TokenListParser<TokenType, Either<string, AstDestructuringPatternNode>> DestructuringBindingAlias { get; } =
        from asKeyword in OperatorParser.CastOperator
        from alias in DestructuringPatternOrIdentifier
        select alias;


    public static TokenListParser<TokenType, AstDestructuringPatternBindingElement> DestructuringBindingElement { get; }
        =
        from name in Token.EqualTo(TokenType.Identifier)
        from alias in DestructuringBindingAlias.OptionalOrDefault()
        select new AstDestructuringPatternBindingElement(name.ToStringValue(), alias);
    
    public static TokenListParser<TokenType, AstDestructuringObjectPetternNode> DestructuringObjectElements { get; } =
        from names in DestructuringBindingElement.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        select new AstDestructuringObjectPetternNode(names.ToArray(), null);
    
    public static TokenListParser<TokenType, AstDestructuringObjectPetternNode> DestructuringObjectElementsWithRest { get; } =
        from names in DestructuringBindingElement.ManyDelimitedBy(Token.EqualTo(TokenType.Comma))
        from comma in Token.EqualTo(TokenType.Comma)
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new AstDestructuringObjectPetternNode(names.ToArray(), rest.ToStringValue());
    
    public static TokenListParser<TokenType, AstDestructuringObjectPetternNode> DestructuringObjectElementsRestOnly { get; } =
        from threeDots in Token.EqualTo(TokenType.DotOperator).Repeat(3)
        from rest in Token.EqualTo(TokenType.Identifier)
        select new AstDestructuringObjectPetternNode(new AstDestructuringPatternBindingElement[] {},
            rest.ToStringValue());

    public static TokenListParser<TokenType, AstDestructuringObjectPetternNode> DestructuringObjectPattern { get; } =
        from openCurly in Token.EqualTo(TokenType.OpenCurly)
        from elements in DestructuringObjectElements
            .Or(DestructuringObjectElementsWithRest)
            .Or(DestructuringObjectElementsRestOnly)
        from closeCurly in Token.EqualTo(TokenType.CloseCurly)
        select elements;

    public static TokenListParser<TokenType, AstDestructuringPatternNode> DestructuringPattern { get; } =
        DestructuringArrayPattern.Select(ap => (AstDestructuringPatternNode) ap)
            .Or(DestructuringTuplePattern.Select(tp => (AstDestructuringPatternNode) tp))
            .Or(DestructuringObjectPattern.Select(op => (AstDestructuringPatternNode) op));

    public static TokenListParser<TokenType, AstDocumentNode> Document { get; } =
        from statements in Parsers.TopLevelStatementBlock
        select new AstDocumentNode(statements);
    
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