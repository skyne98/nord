using System;
using System.Collections.Generic;
using System.Linq;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Nord.Compiler.Lexer
{
    public class NordTokenizer: Tokenizer<NordTokenType>
    {
        static readonly Dictionary<char, NordTokenType> _operators = new Dictionary<char, NordTokenType>
        {
            ['='] = NordTokenType.EqualsOperator,
            ['+'] = NordTokenType.PlusOperator,
            ['-'] = NordTokenType.MinusOperator,
            ['*'] = NordTokenType.StarOperator,
            ['/'] = NordTokenType.ForwardSlashOperator,
            ['\\'] = NordTokenType.BackwardSlashOperator,
            ['?'] = NordTokenType.QuestionMarkOperator,
            ['!'] = NordTokenType.ExclamationMarkOperator,
            ['.'] = NordTokenType.DotOperator,
            ['('] = NordTokenType.OpenParen,
            [')'] = NordTokenType.CloseParen,
            ['{'] = NordTokenType.OpenCurly,
            ['}'] = NordTokenType.CloseCurly,
            ['['] = NordTokenType.OpenSquare,
            [']'] = NordTokenType.CloseSquare,
            ['<'] = NordTokenType.OpenAngle,
            ['>'] = NordTokenType.CloseAngle,
            [','] = NordTokenType.Comma,
            [';'] = NordTokenType.Semicolon,
            [':'] = NordTokenType.Colon,
        };

        static readonly Dictionary<string, NordTokenType> _keywords = new Dictionary<string, NordTokenType>
        {
            ["let"] = NordTokenType.LetKeyword,
            ["fn"] = NordTokenType.FnKeyword,
            ["end"] = NordTokenType.EndKeyword,
            ["if"] = NordTokenType.IfKeyword,
            ["else"] = NordTokenType.ElseKeyword,
            ["loop"] = NordTokenType.LoopKeyword,
            ["return"] = NordTokenType.ReturnKeyword,
            ["break"] = NordTokenType.BreakKeyword,
            ["continue"] = NordTokenType.ContinueKeyword,
            ["when"] = NordTokenType.WhenKeyword,
            ["in"] = NordTokenType.InKeyword,
            ["new"] = NordTokenType.NewKeyword
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

        public static TextParser<char> StringContentCharTokenizer =
            Span.EqualTo("''").Value('\'').Try().Or(Character.ExceptIn('\'', '\r', '\n'));

        public static TextParser<string> StringTokenizer =
            from open in Character.EqualTo('\'')
            from value in StringContentCharTokenizer.Many()
            from close in Character.EqualTo('\'')
            select new string(value);

        protected override IEnumerable<Result<NordTokenType>> Tokenize(TextSpan span)
        {
            var next = SkipWhiteSpace(span);
            if (!next.HasValue)
                yield break;

            do
            {
                NordTokenType charTokenType;

                if (char.IsDigit(next.Value))
                {
                    var result = DoubleTokenizer(next.Location);
                    next = result.Remainder.ConsumeChar();
                    yield return Result.Value(NordTokenType.Double, result.Location, result.Remainder);
                }
                else if (next.Value == '\'')
                {
                    var str = StringTokenizer(next.Location);
                    if (!str.HasValue)
                        yield return Result.CastEmpty<string, NordTokenType>(str);

                    next = str.Remainder.ConsumeChar();
                    yield return Result.Value(NordTokenType.String, str.Location, str.Remainder);
                }
                else if (char.IsLetter(next.Value) || next.Value == '_')
                {
                    var beginIdentifier = next.Location;
                    do
                    {
                        next = next.Remainder.ConsumeChar();
                    }
                    while (next.HasValue && (char.IsLetterOrDigit(next.Value) || next.Value == '_'));

                    NordTokenType keyword;
                    if (TryGetKeyword(beginIdentifier.Until(next.Location), out keyword))
                    {
                        yield return Result.Value(keyword, beginIdentifier, next.Location);
                    }
                    else
                    {
                        yield return Result.Value(NordTokenType.Identifier, beginIdentifier, next.Location);
                    }
                }
                else if (_operators.TryGetValue(next.Value, out charTokenType))
                {
                    yield return Result.Value(charTokenType, next.Location, next.Remainder);
                    next = next.Remainder.ConsumeChar();
                }
                else
                {
                    yield return Result.Empty<NordTokenType>(next.Location, new[] { "number", "operator" });
                }

                next = SkipWhiteSpace(next.Location);
            } while (next.HasValue);
        }

        static bool TryGetKeyword(TextSpan span, out NordTokenType keyword)
        {
            foreach (var kw in _keywords)
            {
                if (span.EqualsValueIgnoreCase(kw.Key))
                {
                    keyword = kw.Value;
                    return true;
                }
            }

            keyword = NordTokenType.None;
            return false;
        }
    }
}
