using Calculator.IO;
using Calculator.Syntax.Tokens;
using Calculator.Utils;

namespace Calculator.State.Methods;

public class ListOperatorsMethod(IHost host, ITypeCollector typeCollector) : IListOperatorsMethod
{
    private readonly IHost _host = host;
    private readonly ITypeCollector _typeCollector = typeCollector;

    public Task<object?> Execute(params object?[] args)
    {
        var binaryOps = _typeCollector.GetBinaryOpTokens(GetType().Assembly);
        var unaryOps = _typeCollector.GetUnaryOpTokens(GetType().Assembly);

        _host.WriteLine("Unary Operators:", ConsoleColor.Yellow);
        ListOperators<IUnaryOperatorToken>(unaryOps, token => token.UnaryPriority);

        _host.WriteLine();

        _host.WriteLine("Binary Operators:", ConsoleColor.Yellow);
        ListOperators<IBinaryOperatorToken>(binaryOps, token => token.BinaryPriority);

        _host.WriteLine();

        return Task.FromResult<object?>(null);
    }

    private void ListOperators<T>(IEnumerable<Type> types, Func<T, int> getPriority)
        where T : class, IOperatorToken
    {
        var tokens = types
            .Select(type => CreateTokenOrNull<T>(type)!)
            .Where(token => token is not null);

        var groups = tokens.GroupBy(token => getPriority(token));

        foreach (var group in groups.OrderBy(group => group.Key))
        {
            _host.WriteLine($" * Priority: {group.Key}", ConsoleColor.White);
            foreach (var @operator in group)
            {
                _host.WriteLine("   - " + @operator.GetType().Name, @operator.DebugColor);
            }
        }
    }

    private T? CreateTokenOrNull<T>(Type type)
        where T : class, IOperatorToken
    {
        var token = Activator.CreateInstance(type);
        if (token is T operatorToken)
        {
            return operatorToken;
        }

        return null;
    }
}
