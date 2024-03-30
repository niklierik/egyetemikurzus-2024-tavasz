namespace Calculator.Source;

public class TextSource(string text) : ITextSource
{
    public string All { get; } = text;
}
