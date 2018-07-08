using System;
using Nord.Compiler.Lexer;
using Nord.Compiler.Parser;
using Superpower;
using Superpower.Model;

public class Parsers
{
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

        var parsed = StatementParser.Statement.TryParse(tokens.Value);
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