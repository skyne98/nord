using System;
using System.Collections.Generic;
using System.Text;
using Superpower.Display;

namespace Nord.Compiler.Lexer
{
    public enum TokenType
    {
        None,

        // Values
        [Token(Category = "value", Example = "integer literal")]
        Integer,
        [Token(Category = "value", Example = "double literal")]
        Double,
        [Token(Category = "value", Example = "identifier")]
        Identifier,
        [Token(Category = "value", Example = "string literal")]
        String,

        // Keywords
        [Token(Category = "keyword", Example = "let")]
        LetKeyword,
        [Token(Category = "keyword", Example = "fn")]
        FnKeyword,
        [Token(Category = "keyword", Example = "end")]
        EndKeyword,
        [Token(Category = "keyword", Example = "if")]
        IfKeyword,
        [Token(Category = "keyword", Example = "else")]
        ElseKeyword,
        [Token(Category = "keyword", Example = "loop")]
        LoopKeyword,
        [Token(Category = "keyword", Example = "return")]
        ReturnKeyword,
        [Token(Category = "keyword", Example = "break")]
        BreakKeyword,
        [Token(Category = "keyword", Example = "continue")]
        ContinueKeyword,
        [Token(Category = "keyword", Example = "when")]
        WhenKeyword,
        [Token(Category = "keyword", Example = "in")]
        InKeyword,
        [Token(Category = "keyword", Example = "new")]
        NewKeyword,
        [Token(Category = "keyword", Example = "as")]
        AsKeyword,
        [Token(Category = "keyword", Example = "class")]
        ClassKeyword,
        [Token(Category = "keyword", Example = "open")]
        OpenKeyword,
        [Token(Category = "keyword", Example = "final")]
        FinalKeyword,
        [Token(Category = "keyword", Example = "abs")]
        AbstractKeyword,
        [Token(Category = "keyword", Example = "use")]
        UseKeyword,
        [Token(Category = "keyword", Example = "pub")]
        PublicKeyword,
        [Token(Category = "keyword", Example = "pri")]
        PrivateKeyword,
        [Token(Category = "keyword", Example = "is")]
        IsKeyword,

        // Operators
        [Token(Category = "operator", Example = "=")]
        EqualsOperator,
        [Token(Category = "operator", Example = "+")]
        PlusOperator,
        [Token(Category = "operator", Example = "-")]
        MinusOperator,
        [Token(Category = "operator", Example = "*")]
        StarOperator,
        [Token(Category = "operator", Example = "/")]
        ForwardSlashOperator,
        [Token(Category = "operator", Example = "\\")]
        BackwardSlashOperator,
        [Token(Category = "operator", Example = "?")]
        QuestionMarkOperator,
        [Token(Category = "operator", Example = "!")]
        ExclamationMarkOperator,
        [Token(Category = "operator", Example = "|")]
        PipeOperator,
        [Token(Category = "operator", Example = "&")]
        AndOperator,
        [Token(Category = "operator", Example = ".")]
        DotOperator,

        // Misc
        [Token(Example = "(")]
        OpenParen,
        [Token(Example = ")")]
        CloseParen,
        [Token(Example = "{")]
        OpenCurly,
        [Token(Example = "}")]
        CloseCurly,
        [Token(Example = "[")]
        OpenSquare,
        [Token(Example = "]")]
        CloseSquare,
        [Token(Example = "<")]
        OpenAngle,
        [Token(Example = ">")]
        CloseAngle,
        [Token(Example = ",")]
        Comma,
        [Token(Example = ";")]
        Semicolon,
        [Token(Example = ":")]
        Colon
    }
}
