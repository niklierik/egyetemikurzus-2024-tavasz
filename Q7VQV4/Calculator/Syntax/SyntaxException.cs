using Calculator.Source;

namespace Calculator.Syntax.Tokens;

public class SyntaxException : Exception
{
    public TextSlice TextSlice { get; } = new("", 0, 0);

    public SyntaxException() { }

    public SyntaxException(string tag, string message, TextSlice textSlice)
        : base(
            $"""
[{tag}] - {message},
Input: 
'{textSlice}'

{textSlice.Text} from {textSlice.Start} to {textSlice.End}

"""
        )
    {
        TextSlice = textSlice;
    }

    public SyntaxException(string? message)
        : base(message) { }

    public SyntaxException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
