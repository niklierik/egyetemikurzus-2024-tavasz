using Calculator.IO;
using Calculator.IO.Logging;
using Calculator.Syntax;
using Calculator.Syntax.Lexing;
using Calculator.Syntax.Tokens;

namespace Calculator;

public static class Program
{
    public static void Main(string[] args)
    {
        ILexer lexer = new Lexer();
        IHost host = new ConsoleHost();
        while (true)
        {
            host.Write(" > ", ConsoleColor.White);
            string? text = host.ReadLine();
            if (text is null)
            {
                return;
            }

            try
            {
                IReadOnlyList<ISyntaxToken> tokens = lexer.LexString(text);

                foreach (var token in tokens)
                {
                    host.Write(token, token.DebugColor);
                }

                host.WriteLine();
            }
            catch (SyntaxException exception)
            {
                host.WriteLine(exception, ConsoleColor.Red);
            }
        }
    }
}
