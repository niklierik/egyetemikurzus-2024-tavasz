namespace Calculator.Syntax.Tokens;

[ConstantStringToken("import")]
public record class ImportToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.Blue;

    public override string ToString() => "import";
}
