using System;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using LanguageExt;
using Nord.Compiler.Ast;
using Nord.Compiler.Lexer;
using Nord.Compiler.Parser;
using Superpower;
using Superpower.Util;
using Superpower.Model;
using Superpower.Parsers;

public static class Parsers
{
    public static TokenListParser<TokenType, AstStatementNode[]> StatementsBlock { get; } =
        from statements in Parse.Ref(() => StatementParser.Statement)
            .ManyDelimitedBy(Token.EqualTo(TokenType.Semicolon).OptionalOrDefault())
        select statements;

    public static TokenListParser<TokenType, AstStatementTopLevelNode[]> TopLevelStatementBlock { get; } = 
        from statements in Parse.Ref(() => StatementParser.TopLevelStatement)
            .ManyDelimitedBy(Token.EqualTo(TokenType.Semicolon).OptionalOrDefault())
        select statements; 
    
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

        var parsed = StatementParser.TopLevelStatement.TryParse(tokens.Value);
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