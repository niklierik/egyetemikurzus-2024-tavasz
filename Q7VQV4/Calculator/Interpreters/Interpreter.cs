using System.Text;
using Calculator.Evaluators;
using Calculator.Evaluators.Exceptions;
using Calculator.IO;
using Calculator.IO.Logging;
using Calculator.State;
using Calculator.State.Methods;
using Calculator.Syntax.AST;
using Calculator.Syntax.Lexing;
using Calculator.Syntax.Parser;
using Calculator.Syntax.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Interpreters;

public class Interpreter : IInterpreter
{
    private readonly ILexer _lexer;
    private readonly IParser _parser;
    private readonly IEvaluator _evaluator;
    private readonly ILogManager _logger;
    private readonly INodePrettyPrinter _nodePrettyPrinter;
    private readonly IHost _host;
    private readonly IConfigLoader<InterpreterConfig> _configLoader;
    private readonly IServiceProvider _serviceProvider;

    public InterpreterState State { get; }

    public Interpreter(
        ILexer lexer,
        IParser parser,
        IEvaluator evaluator,
        ILogManager logger,
        INodePrettyPrinter nodePrettyPrinter,
        IHost host,
        IConfigLoader<InterpreterConfig> configLoader,
        IServiceProvider serviceProvider
    )
    {
        _lexer = lexer;
        _parser = parser;
        _evaluator = evaluator;
        _logger = logger;
        _nodePrettyPrinter = nodePrettyPrinter;
        _host = host;
        _configLoader = configLoader;
        _serviceProvider = serviceProvider;
        State = new InterpreterState()
        {
            Consts = new Dictionary<string, object?>()
            {
                { "true", true },
                { "false", false },
                { "pi", Math.PI },
                { "e", Math.E },
                { "null", null },
                { "interpreter", this },
            },
            Methods = new Dictionary<string, IMethod>()
        };
    }

    public async Task Init()
    {
        State.Config = await _configLoader.Load("interp.json");

        await LoadInitScripts();

        State.Consts["interpreter"] = this;
        State.Consts["state"] = State;
        State.Consts["config"] = State.Config;
        State.Methods["config"] = _serviceProvider.GetRequiredService<IConfigMethod>();
        State.Methods["listOperators"] =
            _serviceProvider.GetRequiredService<IListOperatorsMethod>();
    }

    public async Task<object?> ExecuteFile(string path)
    {
        var lines = File.ReadAllLines(path);
        object? result = null;
        foreach (var line in lines)
        {
            result = await Execute(line);
        }
        return result;
    }

    public async Task<object?> Execute(string line)
    {
        try
        {
            IReadOnlyList<Syntax.ISyntaxToken> tokens = _lexer.LexString(line, true);
            IEnumerable<string> tokensInString = tokens.Select(token =>
                Environment.NewLine + (token.ToString() ?? "")
            );
            await _logger.Debug(string.Join("", tokensInString));

            RootNode ast = _parser.Parse(tokens);
            StringBuilder prettyAstBuilder = new StringBuilder(Environment.NewLine);
            await _nodePrettyPrinter.Print(
                ast,
                (text, color) => PrintAstNode(text, color, prettyAstBuilder)
            );

            await _logger.Debug(prettyAstBuilder.ToString());

            object? result = await _evaluator.Evaluate(ast);
            State.Variables["ANS"] = result;
            return result;
        }
        catch (SyntaxException e)
        {
            _host.WriteLine("Syntax error:", ConsoleColor.Red);
            _host.WriteLine(e.Message, ConsoleColor.Red);
            await _logger.Error(e);
            if (State.Config.PrintStacktraces == PrintStacktracesOptions.All)
            {
                _host.WriteLine(e.StackTrace);
            }
            return e;
        }
        catch (EvaluatorException e)
        {
            _host.WriteLine("Evaluator error (invalid setup):", ConsoleColor.Red);
            _host.WriteLine(e.Message, ConsoleColor.Red);
            await _logger.Error(e);
            if (State.Config.PrintStacktraces == PrintStacktracesOptions.All)
            {
                _host.WriteLine(e.StackTrace);
            }
            return e;
        }
        catch (RuntimeException e)
        {
            _host.WriteLine("Runtime error:", ConsoleColor.Red);
            _host.WriteLine(e.Message, ConsoleColor.Red);
            await _logger.Error(e);
            if (State.Config.PrintStacktraces == PrintStacktracesOptions.All)
            {
                _host.WriteLine(e.StackTrace);
            }
            return e;
        }
        catch (Exception e)
        {
            _host.WriteLine("Unhandled error:");
            _host.WriteLine(e.Message, ConsoleColor.Red);
            await _logger.Error(e);
            if (State.Config.PrintStacktraces > PrintStacktracesOptions.None)
            {
                _host.WriteLine(e.StackTrace);
            }
            return e;
        }
    }

    private Task PrintAstNode(string text, ConsoleColor color, StringBuilder prettyAstBuilder)
    {
        if (State.Config.PrintAstToConsole)
        {
            _host.Write(text, color);
        }
        prettyAstBuilder.Append(text);
        return Task.CompletedTask;
    }

    private async Task LoadInitScripts()
    {
        foreach (var pathFolder in State.Paths)
        {
            var initFile = Path.Join(pathFolder, "init.script");
            if (File.Exists(initFile))
            {
                await ExecuteFile(initFile);
            }
        }
    }
}
