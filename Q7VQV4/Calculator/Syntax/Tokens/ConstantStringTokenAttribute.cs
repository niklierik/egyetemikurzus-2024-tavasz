namespace Calculator.Syntax.Tokens;

public class ConstantStringTokenAttribute(string matchTo) : Attribute
{
    public string MatchTo { get; } = matchTo;
}
