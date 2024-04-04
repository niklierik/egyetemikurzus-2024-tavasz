namespace Calculator.Source;

public sealed record TextSlice(string Text, int Start, int End)
{
    public override string ToString() => Text[Start..End];
}
