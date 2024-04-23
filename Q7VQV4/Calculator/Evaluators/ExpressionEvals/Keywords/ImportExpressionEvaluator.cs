using System.Collections.Immutable;
using Calculator.Evaluators.Exceptions;
using Calculator.Interpreters;
using Calculator.State;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;
using Calculator.Utils;

namespace Calculator.Evaluators.ExpressionEvals.Keywords;

[EvaluatorFor(typeof(ImportExpressionNode))]
public class ImportExpressionEvaluator(IInterpreter interpreter) : ISubEvaluator
{
    public const string Decapitalize = "__decapitalize__";
    public const string Capitalize = "__capitalize__";
    public const string Upper = "__upper__";
    public const string Lower = "__lower__";
    public const string UseFullName = "__fullname__";
    public const string UseClassName = "__classname__";
    public const string UseMethodName = "__methodname__";

    private readonly IInterpreter _interpreter = interpreter;

    public Task<object?> Evaluate(ISyntaxNode arg)
    {
        HashSet<string> knownModifiers = [Decapitalize, Capitalize, Upper, Lower];
        List<string> prefixes = [""];

        if (arg is not ImportExpressionNode importExpressionNode)
        {
            throw new EvaluatorException(
                $"Tried to run import logic on node {arg} which is not an import expression."
            );
        }

        if (importExpressionNode.CSharpClassName.Token is not IdentifierToken id)
        {
            throw new SyntaxException(
                "'import ... from' must be followed with an identifier containing a C# class."
            );
        }

        var type = Type.GetType(id.Content);
        if (type is null)
        {
            throw new MissingNativeObjectException($"Missing C# class '{id.Content}'.");
        }
        prefixes.Add($"{type.Name}.");
        if (type.FullName is not null)
        {
            prefixes.Add($"{type.FullName}.");
        }

        bool isAll = importExpressionNode.Args.Any(node => node.Token is MultiplyToken);

        IEnumerable<(string target, string alias)> imports;

        ImmutableList<string> activeModifiers = importExpressionNode
            .Args.Select(arg => arg.Token is IdentifierToken token ? token.Content : "")
            .Where(mod => knownModifiers.Contains(mod))
            .ToImmutableList();

        if (!isAll)
        {
            imports = importExpressionNode
                .Args.Select(ExtractIdentifierTokenFromLeafNode)
                .Where(arg => !knownModifiers.Contains(arg.Item1));
        }
        else
        {
            imports = type.GetMethods().Select(method => (method.Name, method.Name));
        }

        if (activeModifiers.Contains(Decapitalize))
        {
            imports = imports
                .Select(import => (import.target, import.target.Decapitalize()))
                .ToArray();
        }
        if (activeModifiers.Contains(Capitalize))
        {
            imports = imports
                .Select(import => (import.target, import.target.Capitalize()))
                .ToArray();
        }
        if (activeModifiers.Contains(Lower))
        {
            imports = imports.Select(import => (import.target, import.target.ToLower())).ToArray();
        }
        if (activeModifiers.Contains(Upper))
        {
            imports = imports.Select(import => (import.target, import.target.ToUpper())).ToArray();
        }
        if (
            activeModifiers.Contains(UseFullName)
            || activeModifiers.Contains(UseClassName)
            || activeModifiers.Contains(UseMethodName)
        )
        {
            prefixes.Clear();
        }
        if (activeModifiers.Contains(UseFullName))
        {
            if (type.FullName is not null)
            {
                prefixes.Add($"{type.FullName}.");
            }
        }
        if (activeModifiers.Contains(UseClassName))
        {
            prefixes.Add($"{type.Name}.");
        }
        if (activeModifiers.Contains(UseMethodName))
        {
            prefixes.Add("");
        }

        foreach (var (target, aliasName) in imports)
        {
            foreach (var prefix in prefixes)
            {
                var alias = $"{prefix}{aliasName}";
                _interpreter.State.Methods[alias] = new NativeStaticMethodWrapper()
                {
                    Alias = alias,
                    CSharpClass = type.FullName ?? type.Name,
                    MethodName = target
                };
            }
        }

        return Task.FromResult<object?>(null);
    }

    private static (string, string) ExtractIdentifierTokenFromLeafNode(LeafNode arg)
    {
        var name = arg.Token switch
        {
            IdentifierToken id => id.Content,
            _
                => throw new SyntaxException(
                    "Import statement must be followed with any number and combination of Identifier, a '*' and the last symbol must be a 'from' keyword."
                )
        };
        return (name, name);
    }
}
