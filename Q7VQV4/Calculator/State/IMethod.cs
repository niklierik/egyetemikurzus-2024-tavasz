namespace Calculator.State;

public interface IMethod
{
    public object? Execute(params object?[] args);
}
