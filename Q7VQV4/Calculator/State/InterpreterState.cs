namespace Calculator.State;

public class InterpreterState
{
    public bool PrintAstToConsole { get; set; } = false;

    public Dictionary<string, object> Variables { get; set; } = new();
}
