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
            await logger.Debug(string.Join("", tokensInString));

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

            await logger.Debug(prettyAstBuilder.ToString());

            object? result = _evaluator.Evaluate(ast);
            return result;
        }
        catch (SyntaxException e) { }
        catch (EvaluatorException e) { }
        return null;
    }
}
