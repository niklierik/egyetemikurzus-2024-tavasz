using Calculator.Syntax.AST;

namespace Calculator.IO;

public class NodePrettyPrinter(IHost host) : INodePrettyPrinter
{
    private readonly IHost _host = host;

    public async Task Print(ISyntaxNode node, Printer printer)
    {
        await Print(node, printer, "", true);
    }

    // https://stackoverflow.com/questions/1649027/how-do-i-print-out-a-tree-structure
    private async Task Print(ISyntaxNode node, Printer printer, string prefix, bool last)
    {
        var eol = Environment.NewLine;
        await printer(prefix, ConsoleColor.DarkGray);
        if (last)
        {
            await printer("\\-", ConsoleColor.DarkGray);
            prefix += "  ";
        }
        else
        {
            await printer("|-", ConsoleColor.DarkGray);
            prefix += "| ";
        }
        var stringified = node.ToString() ?? "";

        await printer(stringified + eol, node.DebugColor);

        var children = node.Children.ToList();

        for (int i = 0; i < children.Count; i++)
        {
            await Print(children[i], printer, prefix, i == children.Count - 1);
        }
    }
}
