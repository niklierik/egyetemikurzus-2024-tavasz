namespace Calculator.Syntax.Tokens;

public record TextSlice(string Text, int Start, int End)
{
    public override string ToString() => Text[Start..End];
}
