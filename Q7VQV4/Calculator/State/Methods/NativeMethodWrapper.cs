using System.Reflection;
using Calculator.Evaluators.Exceptions;
using Calculator.NativeMethods;
using Calculator.Syntax.Tokens;

namespace Calculator.State;

public class NativeStaticMethodWrapper : IMethod
{
    public string Alias { get; set; } = "";
    public string CSharpClass { get; set; } = "";
    public string MethodName { get; set; } = "";

    private MethodInfo? GetMethod(Type type, string methodName, object?[] args)
    {
        try
        {
            return type.GetMethod(methodName);
        }
        catch (AmbiguousMatchException)
        {
            var method = type.GetMethod(
                methodName,
                args.Select(arg => arg?.GetType() ?? typeof(void)).ToArray()
            );
            if (method is not null)
            {
                return method;
            }
            if (type != args[0]?.GetType())
            {
                return null;
            }
            args = args[1..];
            return GetMethod(type, methodName, args);
        }
    }

    public Task<object?> Execute(params object?[] args)
    {
        return Task.Run(() =>
        {
            var type = Type.GetType(CSharpClass);
            if (type is null)
            {
                throw new MissingNativeObjectException(
                    $"There is no C# class called '{CSharpClass}' for '{Alias}'."
                );
            }

            MethodInfo? method = GetMethod(type, MethodName, args);
            if (method is null)
            {
                throw new MissingNativeObjectException(
                    $"There is no C# method called '{CSharpClass}.{MethodName}' for {Alias}."
                );
            }

            string argumentTypes = string.Join(
                ", ",
                args.Select(arg =>
                {
                    var type = arg?.GetType() ?? typeof(void);
                    return $"{type} {arg}";
                })
            );

            try
            {
                if (method.IsStatic && method.GetCustomAttribute<VarArgsAttribute>() is not null)
                {
                    return method.Invoke(null, [args]);
                }
                if (method.IsStatic)
                {
                    return method.Invoke(null, args);
                }

                object? target = args[0];
                if (args.Length == 1)
                {
                    return method.Invoke(target, Array.Empty<object>());
                }

                args = args[1..];
                return method.Invoke(target, args);
            }
            catch (Exception exception)
            {
                if (exception is ArgumentException || exception is TargetParameterCountException)
                {
                    throw new SyntaxException(
                        $"Cannot use method {Alias} -> {CSharpClass}.{MethodName} with arguments ({argumentTypes}).",
                        exception
                    );
                }
                throw;
            }
        });
    }
}
