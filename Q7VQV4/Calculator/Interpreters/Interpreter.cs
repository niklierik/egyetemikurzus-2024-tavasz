using System.Text;
using Calculator.Evaluators;
using Calculator.Evaluators.Exceptions;
using Calculator.IO;
using Calculator.IO.Logging;
using Calculator.State;
using Calculator.Syntax.AST;
using Calculator.Syntax.Lexing;
using Calculator.Syntax.Parser;
using Calculator.Syntax.Tokens;

namespace Calculator.Interpreters;

public class Interpreter(
    ILexer lexer,
    IParser parser,
    IEvaluator evaluator,
    ILogManager logger,
    INodePrettyPrinter nodePrettyPrinter,
    IHost host,
    IStateLoader<InterpreterState> stateProvider
) : IInterpreter<InterpreterState>
{
    private readonly ILexer _lexer = lexer;
    private readonly IParser _parser = parser;
    private readonly IEvaluator _evaluator = evaluator;
    private readonly ILogManager _logger = logger;
    private readonly IHost _host = host;
    private readonly IStateLoader<InterpreterState> _stateProvider = stateProvider;

    private InterpreterState? _state;

    public async Task Init()
    {
        _state = await _stateProvider.LoadState("interp.json");
    }

    public InterpreterState State
    {
        get
        {
            if (_state is null)
            {
                throw new InvalidOperationException("Tried to access state before it was loaded.");
            }
            return _state;
        }
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
            await nodePrettyPrinter.Print(
                ast,
                (text, color) =>
                {
                    if (State.PrintAstToConsole)
                    {
                        _host.Write(text, color);
                    }
                    prettyAstBuilder.Append(text);
                    return Task.CompletedTask;
                }
            );

            await _logger.Debug(prettyAstBuilder.ToString());

            object? result = _evaluator.Evaluate(ast);
            return result;
        }
        catch (SyntaxException e)
        {
            _host.WriteLine("Syntax error:", ConsoleColor.Red);
            _host.WriteLine(e.Message, ConsoleColor.Red);
            await _logger.Error(e);
            return e;
        }
        catch (EvaluatorException e)
        {
            _host.WriteLine("Evaluator error (invalid setup):", ConsoleColor.Red);
            _host.WriteLine(e.Message, ConsoleColor.Red);
            await _logger.Error(e);
            return e;
        }
        catch (RuntimeException e)
        {
            _host.WriteLine("Runtime error:", ConsoleColor.Red);
            _host.WriteLine(e.Message, ConsoleColor.Red);
            await _logger.Error(e);
            return e;
        }
        catch (Exception e)
        {
            _host.WriteLine("Unhandled error:");
            _host.WriteLine(e, ConsoleColor.Red);
            await _logger.Error(e);
            return e;
        }
    }
}
