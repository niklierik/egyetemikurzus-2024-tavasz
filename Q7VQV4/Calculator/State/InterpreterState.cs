namespace Calculator.State;

public class InterpreterState
{
    public Dictionary<string, object?> Variables { get; init; } = [];
    public Dictionary<string, object?> Consts { get; init; } = [];

    public InterpreterConfig Config { get; set; } = new();

    public Dictionary<string, IMethod> Methods { get; init; } = [];

    public List<string> Paths { get; init; } = ["scripts"];

    public InterpreterState() { }

    public object? GetVariable(string name)
    {
        object? value = Consts.GetValueOrDefault(name);
        if (value is not null)
        {
            return value;
        }
        return Variables.GetValueOrDefault(name);
    }
}
