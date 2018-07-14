using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Nord.Compiler.Lexer
{
    public class NordTokenizer: Tokenizer<TokenType>
    {
        static readonly Dictionary<char, TokenType> _operators = new Dictionary<char, TokenType>
        {
            ['='] = TokenType.EqualsOperator,
            ['+'] = TokenType.PlusOperator,
            ['-'] = TokenType.MinusOperator,
            ['*'] = TokenType.StarOperator,
            ['/'] = TokenType.ForwardSlashOperator,
            ['\\'] = TokenType.BackwardSlashOperator,
            ['?'] = TokenType.QuestionMarkOperator,
            ['!'] = TokenType.ExclamationMarkOperator,
            ['|'] = TokenType.PipeOperator,
            ['&'] = TokenType.AndOperator,
            ['.'] = TokenType.DotOperator,
            ['('] = TokenType.OpenParen,
            [')'] = TokenType.CloseParen,
            ['{'] = TokenType.OpenCurly,
            ['}'] = TokenType.CloseCurly,
            ['['] = TokenType.OpenSquare,
            [']'] = TokenType.CloseSquare,
            ['<'] = TokenType.OpenAngle,
            ['>'] = TokenType.CloseAngle,
            [','] = TokenType.Comma,
            [';'] = TokenType.Semicolon,
            [':'] = TokenType.Colon
        };

        static readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
        {
            ["let"] = TokenType.LetKeyword,
            ["fn"] = TokenType.FnKeyword,
            ["end"] = TokenType.EndKeyword,
            ["if"] = TokenType.IfKeyword,
            ["else"] = TokenType.ElseKeyword,
            ["loop"] = TokenType.LoopKeyword,
            ["return"] = TokenType.ReturnKeyword,
            ["break"] = TokenType.BreakKeyword,
            ["continue"] = TokenType.ContinueKeyword,
            ["when"] = TokenType.WhenKeyword,
            ["in"] = TokenType.InKeyword,
            ["new"] = TokenType.NewKeyword,
            ["as"] = TokenType.AsKeyword,
        };

        public static TextParser<double> DoubleTokenizer { get; } =
            from whole in Numerics.Integer.Select(n => double.Parse(n.ToStringValue()))
            from frac in Character.EqualTo('.')
                .IgnoreThen(Numerics.Integer)
                .Select(n => double.Parse(n.ToStringValue()) * Math.Pow(10, -n.Length))
                .OptionalOrDefault()
            from exp in Character.EqualToIgnoreCase('e')
                .IgnoreThen(Character.EqualTo('+').Value(1.0)
                    .Or(Character.EqualTo('-').Value(-1.0))
                    .OptionalOrDefault(1.0))
                .Then(expsign => Numerics.Integer.Select(n => double.Parse(n.ToStringValue()) * expsign))
                .OptionalOrDefault()
            select (whole + frac) * Math.Pow(10, exp);

        public static TextParser<int> IntTokenizer { get; } =
            from whole in Numerics.Integer.Select(n => int.Parse(n.ToStringValue()))
            from exp in Character.EqualToIgnoreCase('e')
                .IgnoreThen(Character.EqualTo('+').Value(1.0)
                    .Or(Character.EqualTo('-').Value(-1.0))
                    .OptionalOrDefault(1.0))
                .Then(expsign => Numerics.Integer.Select(n => int.Parse(n.ToStringValue()) * expsign))
                .OptionalOrDefault()
            select (int)(whole * Math.Pow(10, exp));

        public static TextParser<char> ControlChar =
            from first in Character.EqualTo('\\')
            from next in Character.EqualTo('t').Select(e => '\t')
                .Or(Character.EqualTo('r').Select(e => '\r'))
                .Or(Character.EqualTo('n').Select(e => '\n'))
                .Or(Character.EqualTo('f').Select(e => '\f'))
                .Or(Character.EqualTo('b').Select(e => '\b'))
            select next;
        
        public static TextParser<char> StringContentCharTokenizer =
            Character.ExceptIn('"', '\\').Or(ControlChar);

        public static TextParser<string> StringTokenizer =
            from open in Character.EqualTo('"')
            from value in StringContentCharTokenizer.Many()
            from close in Character.EqualTo('"')
            select new string(value);

        protected override IEnumerable<Result<TokenType>> Tokenize(TextSpan span)
        {
            var next = SkipWhiteSpace(span);
            if (!next.HasValue)
                yield break;

            do
            {
                TokenType charTokenTye;

                if (char.IsDigit(next.Value))
                {
                    var result = DoubleTokenizer(next.Location);
                    next = result.Remainder.ConsumeChar();
                    yield return Result.Value(TokenType.Double, result.Location, result.Remainder);
                }
                else if (next.Value == '\"')
                {
                    var str = StringTokenizer(next.Location);
                    if (!str.HasValue)
                        yield return Result.CastEmpty<string, TokenType>(str);

                    next = str.Remainder.ConsumeChar();
                    yield return Result.Value(TokenType.String, str.Location, str.Remainder);
                }
                else if (char.IsLetter(next.Value) || next.Value == '_')
                {
                    var beginIdentifier = next.Location;
                    do
                    {
                        next = next.Remainder.ConsumeChar();
                    }
                    while (next.HasValue && (char.IsLetterOrDigit(next.Value) || next.Value == '_'));

                    TokenType keyword;
                    if (TryGetKeyword(beginIdentifier.Until(next.Location), out keyword))
                    {
                        yield return Result.Value(keyword, beginIdentifier, next.Location);
                    }
                    else
                    {
                        yield return Result.Value(TokenType.Identifier, beginIdentifier, next.Location);
                    }
                }
                else if (_operators.TryGetValue(next.Value, out charTokenTye))
                {
                    yield return Result.Value(charTokenTye, next.Location, next.Remainder);
                    next = next.Remainder.ConsumeChar();
                }
                else
                {
                    yield return Result.Empty<TokenType>(next.Location, new[] { "number", "operator" });
                }

                next = SkipWhiteSpace(next.Location);
            } while (next.HasValue);
        }

        static bool TryGetKeyword(TextSpan span, out TokenType keyword)
        {
            foreach (var kw in _keywords)
            {
                if (span.EqualsValueIgnoreCase(kw.Key))
                {
                    keyword = kw.Value;
                    return true;
                }
            }

            keyword = TokenType.None;
            return false;
        }
    }
}
