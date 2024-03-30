using Calculator.IO;
using Calculator.IO.Logging;
using Calculator.Syntax;
using Calculator.Syntax.AST;
using Calculator.Syntax.Lexing;
using Calculator.Syntax.Parser;
using Calculator.Syntax.Tokens;

namespace Calculator;

public static class Program
{
    public static void Main(string[] args)
    {
        ILexer lexer = new Lexer();
        IHost host = new ConsoleHost();
        IParser parser = new Parser();
        INodePrettyPrinter nodePrettyPrinter = new NodePrettyPrinter(host);
        while (true)
        {
            host.Write(" > ", ConsoleColor.White);
            string? text = host.ReadLine();
            if (text is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                continue;
            }

            try
            {
                IReadOnlyList<ISyntaxToken> tokens = lexer.LexString(text, true);
                RootNode expr = parser.Parse(tokens);

                foreach (var token in tokens)
                {
                    host.Write(token, token.DebugColor);
                }

                host.WriteLine();

                nodePrettyPrinter.Print(expr);
            }
            catch (SyntaxException exception)
            {
                host.WriteLine(exception, ConsoleColor.Red);
            }
        }
    }
}
