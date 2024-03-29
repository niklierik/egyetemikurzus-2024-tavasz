namespace Calculator.Syntax.Lexing;

public interface ILexer
{
    IReadOnlyList<ISyntaxToken> LexString(string text, bool filterWhitespace = false);
}
