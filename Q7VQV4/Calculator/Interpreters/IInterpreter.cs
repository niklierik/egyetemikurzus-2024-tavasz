using Calculator.State;

namespace Calculator.Interpreters;

public interface IInterpreter
{
    Task Init();
    Task<object?> Execute(string line);
    Task<object?> ExecuteFile(string path);

    InterpreterState State { get; }
}
