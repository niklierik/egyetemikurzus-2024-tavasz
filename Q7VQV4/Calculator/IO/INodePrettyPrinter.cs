using Calculator.Syntax.AST;

namespace Calculator.IO;

public interface INodePrettyPrinter
{
    Task Print(ISyntaxNode node, Printer printer);
}

public delegate Task Printer(string text, ConsoleColor color);
