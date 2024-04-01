using System.Text;
using Calculator.Evaluators;
using Calculator.IO;
using Calculator.IO.Logging;
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
    IHost host
) : IInterpreter
{
    private readonly ILexer _lexer = lexer;
    private readonly IParser _parser = parser;
    private readonly IEvaluator _evaluator = evaluator;
    private readonly ILogManager _logger = logger;
    private readonly IHost _host = host;
    private bool printAstToConsole = false;

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
                    if (printAstToConsole)
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
        }
        catch (EvaluatorException e)
        {
            _host.WriteLine("Evaluator error:", ConsoleColor.Red);
            _host.WriteLine(e.Message, ConsoleColor.Red);
            await _logger.Error(e);
        }
        catch (Exception e)
        {
            _host.WriteLine("Runtime error:");
            _host.WriteLine(e, ConsoleColor.Red);
            await _logger.Error(e);
        }
        return null;
    }
}
