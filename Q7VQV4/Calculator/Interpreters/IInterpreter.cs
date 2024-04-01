namespace Calculator.Interpreters;

public interface IInterpreter<TState>
{
    Task Init();
    Task<object?> Execute(string line);

    TState State { get; }
}
