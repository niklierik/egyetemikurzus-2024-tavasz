using System.Globalization;
using Calculator.Source;
using Calculator.Syntax.Tokens;

namespace Calculator.Syntax.Lexing;

public class Lexer : ILexer
{
    private int _currentPosition = 0;
    private string _text = "";

    private char CharacterAt(int position)
    {
        if (position < 0)
        {
            return '\0';
        }
        if (position >= _text.Length)
        {
            return '\0';
        }

        return _text[position];
    }

    private char CurrentChar => CharacterAt(_currentPosition);

    private IdentifierToken? LexIdentifier()
    {
        if (!(char.IsAsciiLetter(CurrentChar) || CurrentChar == '_'))
        {
            return null;
        }

        string identifier = "";

        while (char.IsAsciiLetterOrDigit(CurrentChar) || CurrentChar == '_')
        {
            identifier += CurrentChar;
            _currentPosition++;
        }

        return new IdentifierToken(identifier);
    }

    private WhitespaceToken? LexWhitespace()
    {
        if (!char.IsWhiteSpace(CurrentChar))
        {
            return null;
        }

        while (char.IsWhiteSpace(CurrentChar))
        {
            _currentPosition++;
        }

        return new WhitespaceToken();
    }

    private NumberLiteralToken? LexNumber()
    {
        if (!char.IsDigit(CurrentChar))
        {
            return null;
        }

        string rawValue = "";
        int start = _currentPosition;
        while (CurrentChar == '.' || char.IsDigit(CurrentChar))
        {
            rawValue += CurrentChar;
            _currentPosition++;
        }

        if (!double.TryParse(rawValue, CultureInfo.InvariantCulture.NumberFormat, out double value))
        {
            throw new LexerException(
                $"Failed to convert '{rawValue}' into number.",
                new TextSlice(_text, start, _currentPosition)
            );
        }

        return new NumberLiteralToken(value, rawValue);
    }

    private ISyntaxToken? LexOperator()
    {
        switch (CurrentChar)
        {
            case '+':
                _currentPosition++;
                return new AddToken();
            case '-':
                _currentPosition++;
                return new SubtractToken();
            case '*':
                _currentPosition++;
                return new MultiplyToken();
            case '/':
                _currentPosition++;
                return new DivideToken();
            case '(':
                _currentPosition++;
                return new OpenBracketToken();
            case ')':
                _currentPosition++;
                return new CloseBracketToken();
            default:
                return null;
        }
    }

    private ISyntaxToken LexBadToken()
    {
        BadToken badToken = new BadToken(_text[_currentPosition].ToString());
        _currentPosition++;
        return badToken;
    }

    private ISyntaxToken NextToken()
    {
        if (_currentPosition >= _text.Length)
        {
            return new EndToken(true);
        }

        ISyntaxToken? token = null;

        token ??= LexOperator();
        token ??= LexWhitespace();
        token ??= LexIdentifier();
        token ??= LexNumber();
        token ??= LexBadToken();

        return token;
    }

    public IReadOnlyList<ISyntaxToken> LexString(string text, bool filterWhitespace = false)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [new EndToken(true)];
        }

        _currentPosition = 0;
        _text = text;

        var list = new List<ISyntaxToken>();

        while (list.LastOrDefault() is not EndToken)
        {
            var nextToken = NextToken();

            if (!filterWhitespace || nextToken is not WhitespaceToken)
            {
                list.Add(nextToken);
            }
        }

        return list;
    }
}
