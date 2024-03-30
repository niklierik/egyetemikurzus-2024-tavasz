using Calculator.Source;
using Calculator.Syntax.Tokens;

namespace Calculator.Syntax.Lexing;

public class LexerException : SyntaxException
{
    public LexerException() { }

    public LexerException(string? message)
        : base(message) { }

    public LexerException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public LexerException(string message, TextSlice textSlice)
        : base("Lexer", message, textSlice) { }

    public LexerException(string tag, string message, TextSlice textSlice)
        : base(tag, message, textSlice) { }
}
