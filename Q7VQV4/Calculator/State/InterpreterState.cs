namespace Calculator.State;

public class InterpreterState
{
    public bool PrintAstToConsole { get; set; } = false;

    public Dictionary<string, object?> Variables { get; set; } = [];
    public Dictionary<string, object?> Consts { get; set; } =
        new()
        {
            { "true", true },
            { "false", false },
            { "pi", Math.PI },
            { "e", Math.E },
            { "null", null }
        };

    public Dictionary<string, IMethod> Methods { get; set; } = [];

    public InterpreterState()
    {
        Methods.Add(
            "sin",
            new NativeStaticMethodWrapper()
            {
                Alias = "sin",
                CSharpClass = $"{nameof(System)}.{nameof(Math)}",
                MethodName = "Sin"
            }
        );
    }

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
