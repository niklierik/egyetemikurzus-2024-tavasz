namespace Calculator.Source;

public class FileSource(string path) : ITextSource
{
    public string All { get; } = File.ReadAllText(path);
}
