namespace Calculator.State;

public record class InterpreterConfig
{
    public bool PrintAstToConsole { get; } = false;

    public PrintStacktracesOptions PrintStacktraces { get; } = PrintStacktracesOptions.None;
}
