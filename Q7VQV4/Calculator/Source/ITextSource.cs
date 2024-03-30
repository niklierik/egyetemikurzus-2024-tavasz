namespace Calculator.Source;

public interface ITextSource
{
    public string All { get; }

    public TextSlice SliceRange(int start, int end) => new(All, start, end);

    public TextSlice SliceFixedLength(int from, int length) => new(All, from, from + length);
}
