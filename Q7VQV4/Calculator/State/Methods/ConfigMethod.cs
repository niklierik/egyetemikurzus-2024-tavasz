using System.Reflection;
using Calculator.Evaluators.Exceptions;
using Calculator.Interpreters;

namespace Calculator.State.Methods;

public class ConfigMethod(
    IInterpreter interpreter,
    IConfigLoader<InterpreterConfig> configLoader,
    IJsonService jsonService
) : IConfigMethod
{
    private InterpreterConfig Config => interpreter.State.Config;

    public async Task<object?> Execute(params object?[] args)
    {
        if (args.Length == 0)
        {
            return Config;
        }

        if (args.Length == 1)
        {
            if (args[0] is not string key)
            {
                throw new TypeException(ErrorText(args));
            }
            var prop = PropertyOf(key);
            return prop.GetValue(Config);
        }

        if (args.Length == 2)
        {
            if (args[0] is not string key)
            {
                throw new TypeException();
            }

            if (key == "save")
            {
                await Save(args);
                return Config;
            }

            if (key == "load")
            {
                await Load(args);
                return Config;
            }

            var prop = PropertyOf(key);
            prop.SetValue(Config, args[1]);
            return Config;
        }

        throw new TypeException(ErrorText(args));
    }

    private async Task Load(object?[] args)
    {
        if (args[1] is not string path)
        {
            throw new TypeException(ErrorText(args));
        }

        interpreter.State.Config = await configLoader.Load(path);
    }

    private async Task Save(object?[] args)
    {
        if (args[1] is not string path)
        {
            throw new TypeException(ErrorText(args));
        }

        await configLoader.Save(path, Config);
    }

    private PropertyInfo PropertyOf(string key)
    {
        var prop = interpreter.State.Config.GetType().GetProperty(key);
        if (prop is null)
        {
            throw new TypeException($"Config has no known member '{key}'.");
        }

        return prop;
    }

    private string ErrorText(object?[] args) =>
        $"Method 'config' has no signature that is compatible with these args: {ArgumentsString(args)}";

    private string ArgumentsString(object?[] args) =>
        $"({string.Join(", ", args.Select(arg => $"{jsonService.ToJson(arg)}: {arg?.GetType().ToString() ?? "void"}"))})";
}
