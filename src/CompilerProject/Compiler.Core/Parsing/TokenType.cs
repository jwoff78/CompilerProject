namespace Compiler.Core.Parsing
{
    public enum TokenType
    {
        // ReSharper disable once InconsistentNaming
        UNKNOWN,
        Sof,
        Eof,

        Identifier,
        Number,

        DoubleEq,
        Eq,
        Plus,
        Minus,
        Divide,
        Multiply,

        OpenRoundBracket,
        CloseRoundBracket
    }
}