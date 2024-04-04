namespace Calculator.State;

public sealed record class InterpreterConfig
{
    public bool PrintAstToConsole { get; } = false;

    public PrintStacktracesOptions PrintStacktraces { get; } = PrintStacktracesOptions.None;
}
