using Nord.Compiler.Lexer;
using Superpower;
using Superpower.Parsers;

namespace Nord.Compiler.Parser
{
    public class OperatorParser
    {
        // =
        public static TokenListParser<TokenType, string> AssignmentOperator { get; } =
            from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
            select "=";
        
        // ||
        public static TokenListParser<TokenType, string> LogicalOrOperator { get; } =
            from pipeOperator in Token.EqualTo(TokenType.PipeOperator)
            from otherPipeOperator in Token.EqualTo(TokenType.PipeOperator)
            select "||";
        
        // &&
        public static TokenListParser<TokenType, string> LogicalAndOperator { get; } =
            from andOperator in Token.EqualTo(TokenType.AndOperator)
            from otherAndOperator in Token.EqualTo(TokenType.AndOperator)
            select "&&";
        
        // !=
        public static TokenListParser<TokenType, string> NotEqualsOperator { get; } =
            from exclamationMark in Token.EqualTo(TokenType.ExclamationMarkOperator)
            from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
            select "!=";
        
        // ==
        public static TokenListParser<TokenType, string> EqualsOperator { get; } =
            from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
            from otherEqualsOperator in Token.EqualTo(TokenType.EqualsOperator)
            select "==";
        
        // <
        public static TokenListParser<TokenType, string> LessThanOperator { get; } =
            from openAngle in Token.EqualTo(TokenType.OpenAngle)
            select "<";
        
        // >
        public static TokenListParser<TokenType, string> MoreThanOperator { get; } =
            from closeAngle in Token.EqualTo(TokenType.CloseAngle)
            select ">";
        
        // <=
        public static TokenListParser<TokenType, string> LessThanEqualsOperator { get; } =
            from openAngle in Token.EqualTo(TokenType.OpenAngle)
            from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
            select "<=";
        
        // >=
        public static TokenListParser<TokenType, string> MoreThanEqualsOperator { get; } =
            from closeAngle in Token.EqualTo(TokenType.CloseAngle)
            from equalsOperator in Token.EqualTo(TokenType.EqualsOperator)
            select ">=";
        
        // +
        public static TokenListParser<TokenType, string> PlusOperator { get; } =
            from plusOperator in Token.EqualTo(TokenType.PlusOperator)
            select "+";
        
        // -
        public static TokenListParser<TokenType, string> MinusOperator { get; } =
            from minusOperator in Token.EqualTo(TokenType.MinusOperator)
            select "-";
        
        // *
        public static TokenListParser<TokenType, string> MultiplyOperator { get; } =
            from starOperator in Token.EqualTo(TokenType.StarOperator)
            select "*";
        
        // /
        public static TokenListParser<TokenType, string> DivideOperator { get; } =
            from forwardSlashOperator in Token.EqualTo(TokenType.ForwardSlashOperator)
            select "/";
        
        // as
        public static TokenListParser<TokenType, string> CastOperator{ get; } =
            from asKeyword in Token.EqualTo(TokenType.AsKeyword)
            select "as";
        
        // !
        public static TokenListParser<TokenType, string> BangOperator { get; } =
            from exclamationMarkKeyword in Token.EqualTo(TokenType.ExclamationMarkOperator)
            select "!";
    }
}