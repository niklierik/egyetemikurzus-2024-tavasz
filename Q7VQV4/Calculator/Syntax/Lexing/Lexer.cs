using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
using Calculator.State;
using Calculator.Syntax.Tokens;
using Calculator.Utils;
using Newtonsoft.Json;

namespace Calculator.Syntax.Lexing;

internal record class ConstantStringType
{
    public Type Type { get; }
    public string MatchTo { get; }

    public ConstantStringType(Type type)
    {
        var attribute = type.GetCustomAttribute<ConstantStringTokenAttribute>();
        if (attribute is null)
        {
            // this should never trigger
            throw new InvalidOperationException(
                $"Unable to load lexer as the token marked constant string misses the attribute: {type}"
            );
        }
        Type = type;
        MatchTo = attribute.MatchTo;
    }
}

// TODO: remove state
public class Lexer : ILexer
{
    private readonly IReadOnlyList<ConstantStringType> _constantStringTokens;
    private readonly IJsonService _jsonService;
    private int _currentPosition = 0;
    private string _text = "";
    private char CurrentChar => CharacterAt(_currentPosition);

    public Lexer(ITypeCollector typeCollector, IJsonService jsonService)
    {
        _constantStringTokens = typeCollector
            .GetConstantStringTokens(GetType().Assembly)
            .Select(type => new ConstantStringType(type))
            .OrderByDescending(constantStringType => constantStringType.MatchTo.Length)
            .ToImmutableList();
        _jsonService = jsonService;
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

    private ISyntaxToken? LexIdentifier()
    {
        if (!(char.IsAsciiLetter(CurrentChar) || CurrentChar == '_'))
        {
            return null;
        }

        string identifier = "";

        while (char.IsAsciiLetterOrDigit(CurrentChar) || CurrentChar == '_' || CurrentChar == '.')
        {
            identifier += CurrentChar;
            _currentPosition++;
        }

        var constantStringType = _constantStringTokens.FirstOrDefault(
            (tuple) => tuple.MatchTo == identifier
        );
        if (constantStringType is not null)
        {
            var instance = Activator.CreateInstance(constantStringType.Type);
            if (instance is ISyntaxToken token)
            {
                return token;
            }
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
        while (CurrentChar == '.' || char.IsDigit(CurrentChar))
        {
            rawValue += CurrentChar;
            _currentPosition++;
        }

        if (!double.TryParse(rawValue, CultureInfo.InvariantCulture.NumberFormat, out double value))
        {
            throw new SyntaxException($"Failed to convert '{rawValue}' into number.");
        }

        return new NumberLiteralToken(value, rawValue);
    }

    private ISyntaxToken? LexStringLiteral()
    {
        if (CurrentChar != '\"')
        {
            return null;
        }

        string rawValue = "\"";
        _currentPosition++;
        bool breaking = false;
        while (breaking || CurrentChar != '\"')
        {
            if (CurrentChar == '\\')
            {
                breaking = true;
                rawValue += CurrentChar;
                _currentPosition++;
            }

            rawValue += CurrentChar;
            _currentPosition++;
        }
        rawValue += "\"";
        _currentPosition++;
        try
        {
            var parsed = _jsonService.FromJson<string>(rawValue);

            if (parsed is null)
            {
                throw new SyntaxException($"Failed to parse '{rawValue}' as string.");
            }
            return new StringLiteralToken(parsed, rawValue);
        }
        catch (JsonReaderException e)
        {
            throw new SyntaxException($"Failed to parse '{rawValue}' as string.", e);
        }
    }

    private ISyntaxToken? LexConstantStringToken()
    {
        foreach (var constantStringType in _constantStringTokens)
        {
            var type = constantStringType.Type;
            var matchTo = constantStringType.MatchTo;
            if (_currentPosition + matchTo.Length > _text.Length)
            {
                continue;
            }
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

        token ??= LexWhitespace();
        token ??= LexIdentifier();
        token ??= LexNumber();
        token ??= LexStringLiteral();
        token ??= LexConstantStringToken();
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
