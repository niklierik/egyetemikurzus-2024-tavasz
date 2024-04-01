using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
using Calculator.Source;
using Calculator.Syntax.Tokens;
using Calculator.Utils;

namespace Calculator.Syntax.Lexing;

// TODO: remove state
public class Lexer : ILexer
{
    private readonly IReadOnlyList<(Type type, string toMatch)> _constantStringTokens;
    private int _currentPosition = 0;
    private string _text = "";
    private char CurrentChar => CharacterAt(_currentPosition);

    public Lexer(ITypeCollector typeCollector)
    {
        _constantStringTokens = typeCollector
            .GetConstantStringTokens(GetType().Assembly)
            .Select(type =>
            {
                var attribute = type.GetCustomAttribute<ConstantStringTokenAttribute>();
                if (attribute is null)
                {
                    // this should never trigger
                    throw new InvalidOperationException(
                        $"Unable to load lexer as the token marked constant string misses the attribute: {type}"
                    );
                }
                return (type: type, toMatch: attribute.MatchTo);
            })
            .OrderByDescending(tuple => tuple.toMatch.Length)
            .ToImmutableList();
    }

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

    private ISyntaxToken? LexConstantStringToken()
    {
        foreach (var (type, matchTo) in _constantStringTokens)
        {
            if (matchTo == _text.Substring(_currentPosition, matchTo.Length))
            {
                var tokenInstance = Activator.CreateInstance(type);
                if (tokenInstance is not ISyntaxToken token)
                {
                    throw new InvalidOperationException(
                        $"Created instance from {type} but it cannot be casted into ISyntaxToken."
                    );
                }
                _currentPosition += matchTo.Length;
                return token;
            }
        }
        return null;
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

        token ??= LexConstantStringToken();
        token ??= LexWhitespace();
        token ??= LexIdentifier();
        token ??= LexNumber();
        token ??= LexBadToken();

        return token;
    }

    public IReadOnlyList<ISyntaxToken> LexString(string text, bool filterWhitespace = false)
    {
        try
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
        finally
        {
            _currentPosition = 0;
            _text = "";
        }
    }
}
