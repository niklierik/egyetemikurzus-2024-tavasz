namespace Calculator.State;

public class NativeStaticMethodWrapper : IMethod
{
    public string Alias { get; set; } = "";
    public string CSharpClass { get; set; } = "";
    public string MethodName { get; set; } = "";

    public object? Execute(params object?[] args)
    {
        var type = Type.GetType(CSharpClass);
        if (type is null)
        {
            throw new MissingMethodException(
                $"There is no C# class called '{CSharpClass}' for '{Alias}'."
            );
        }

        var method = type.GetMethod(MethodName);
        if (method is null)
        {
            throw new MissingMethodException(
                $"There is no C# method called '{CSharpClass}.{MethodName}' for {Alias}."
            );
        }

        return method.Invoke(null, args);
    }
}
