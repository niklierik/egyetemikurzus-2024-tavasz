using Calculator.Syntax.AST;

namespace Calculator.IO;

public class NodePrettyPrinter(IHost host) : INodePrettyPrinter
{
    private readonly IHost _host = host;

    public void Print(ISyntaxNode node)
    {
        Print(node, "", true);
    }

    // https://stackoverflow.com/questions/1649027/how-do-i-print-out-a-tree-structure
    private void Print(ISyntaxNode node, string prefix, bool last)
    {
        _host.Write(prefix, ConsoleColor.DarkGray);
        if (last)
        {
            _host.Write("\\-", ConsoleColor.DarkGray);
            prefix += "  ";
        }
        else
        {
            _host.Write("|-", ConsoleColor.DarkGray);
            prefix += "| ";
        }
        _host.WriteLine(node.ToString(), node.DebugColor);

        var children = node.Children.ToList();

        for (int i = 0; i < children.Count; i++)
        {
            Print(children[i], prefix, i == children.Count - 1);
        }
    }
}
