namespace Calculator.Interpreters;

public interface IInterpreter
{
    Task<object?> Execute(string line);
}
