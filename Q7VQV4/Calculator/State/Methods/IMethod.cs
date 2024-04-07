namespace Calculator.State;

public interface IMethod
{
    public Task<object?> Execute(params object?[] args);
}
