namespace Calculator.Interpreters;

public interface IInterpreter<TState>
{
    Task Init();
    Task<object?> Execute(string line);
    Task<object?> ExecuteFile(string path);

    TState State { get; }
}
