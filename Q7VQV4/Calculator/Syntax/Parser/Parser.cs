using System.Collections.Immutable;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Syntax.Parser;

// TODO: remove state
public class Parser() : IParser
{
    private IReadOnlyList<ISyntaxToken> _tokens = Enumerable.Empty<ISyntaxToken>().ToList();
    private int _position = 0;
    private int _minOperandPrio = -1;
    private int _maxOperandPrio = -1;
    private bool _hasOperands = false;

    public RootNode Parse(IReadOnlyList<ISyntaxToken> tokens)
    {
        try
        {
            _position = 0;
            _tokens = tokens.Where(token => token is not EndToken).ToImmutableList();

            if (!_tokens.Any())
            {
                return new RootNode([]);
            }

            var operandsByPrio = _tokens
                .Where(token => token is IOperandToken)
                .Cast<IOperandToken>()
                .Select(operand => operand.Priority);

            _hasOperands = operandsByPrio.Any();

            if (_hasOperands)
            {
                _minOperandPrio = operandsByPrio.Min();
                _maxOperandPrio = operandsByPrio.Max();
            }
            var list = new List<ISyntaxNode>();

            while (true)
            {
                var node = ParseNextExpression();

                if (node is not null)
                {
                    list.Add(node);
                    continue;
                }

                if (_position >= _tokens.Count)
                {
                    return new RootNode(list);
                }

                throw new SyntaxException("Unable to parse meaningful node.");
            }
        }
        finally
        {
            _position = 0;
            _minOperandPrio = -1;
            _maxOperandPrio = -1;
            _hasOperands = false;
            _tokens = Enumerable.Empty<ISyntaxToken>().ToList();
        }
    }

    private ISyntaxNode? ParseNextExpression()
    {
        if (_position >= _tokens.Count)
        {
            return null;
        }

        return ParseNext(_minOperandPrio);
    }

    private ISyntaxNode? ParseNext(int prio)
    {
        if (_position >= _tokens.Count)
        {
            return null;
        }

        ISyntaxNode? syntaxNode = null;
        syntaxNode ??= ParseBinaryExpression(prio);
        syntaxNode ??= ParseLeaf();
        return syntaxNode;
    }

    private LeafNode? ParseLeaf()
    {
        if (_position >= _tokens.Count)
        {
            return null;
        }
        var token = _tokens[_position];
        _position++;
        return new LeafNode(token);
    }

    private ISyntaxNode? ParseBinaryExpression(int level)
    {
        if (!_hasOperands)
        {
            return null;
        }
        if (level > _maxOperandPrio)
        {
            return null;
        }
        ISyntaxNode? left = ParseNext(level + 1);
        if (left is null)
        {
            return null;
        }

        LeafNode? operand = ParseLeaf();

        if (operand is null)
        {
            return left;
        }

        if (operand.LiteralToken is not IOperandToken operandToken || operandToken.Priority < level)
        {
            _position--;
            return left;
        }

        ISyntaxNode? right = ParseNext(level);

        if (right is null)
        {
            throw new SyntaxException("Unable to parse Binary Expression. Missing right.");
        }

        return new BinaryExpressionNode(left, operand, right);
    }
}
