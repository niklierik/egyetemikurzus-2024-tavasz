namespace Calculator.State;

public record class InterpreterState
{
    public Dictionary<string, object?> Variables { get; init; } = [];
    public Dictionary<string, object?> Consts { get; init; } = [];

    public InterpreterConfig Config { get; init; } = new();

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

    public static InterpreterState Default()
    {
        return new()
        {
            Consts = new Dictionary<string, object?>()
            {
                { "true", true },
                { "false", false },
                { "pi", Math.PI },
                { "e", Math.E },
                { "null", null }
            }
        };
    }
}
