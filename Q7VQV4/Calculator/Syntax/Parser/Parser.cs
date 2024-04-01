using System.Collections.Immutable;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Syntax.Parser;

// TODO: remove state
public class Parser : IParser
{
    private IReadOnlyList<ISyntaxToken> _tokens = Enumerable.Empty<ISyntaxToken>().ToList();
    private int _position = 0;
    private int _minOperatorPriority = int.MaxValue;
    private int _maxOperatorPriority = int.MinValue;
    private bool _hasBinaryOperator = false;
    private bool _hasUnaryOperator = false;

    private ISyntaxToken? Current
    {
        get
        {
            if (_position >= _tokens.Count)
            {
                return null;
            }
            return _tokens[_position];
        }
    }

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

            SetOperatorInfos();

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
            _tokens = Enumerable.Empty<ISyntaxToken>().ToList();
            _minOperatorPriority = int.MaxValue;
            _maxOperatorPriority = int.MinValue;
            _hasBinaryOperator = false;
            _hasUnaryOperator = false;
        }
    }

    private void SetOperatorInfos()
    {
        var binaryOperandsByPriority = _tokens
            .Where(token => token is IBinaryOperatorToken)
            .Cast<IBinaryOperatorToken>()
            .Select(operand => operand.BinaryPriority);

        _hasBinaryOperator = binaryOperandsByPriority.Any();

        if (_hasBinaryOperator)
        {
            _minOperatorPriority = binaryOperandsByPriority.Min();
            _maxOperatorPriority = binaryOperandsByPriority.Max();
        }

        var unaryOperandsByPriority = _tokens
            .Where(token => token is IUnaryOperatorToken)
            .Cast<IUnaryOperatorToken>()
            .Select(operand => operand.UnaryPriority);

        _hasUnaryOperator = unaryOperandsByPriority.Any();

        if (_hasUnaryOperator)
        {
            _minOperatorPriority = Math.Min(unaryOperandsByPriority.Min(), _minOperatorPriority);
            _maxOperatorPriority = Math.Max(unaryOperandsByPriority.Max(), _maxOperatorPriority);
        }
    }

    private ISyntaxNode? ParseNextExpression()
    {
        if (_position >= _tokens.Count)
        {
            return null;
        }

        return ParseNext(_minOperatorPriority);
    }

    private ISyntaxNode? ParseNext(int priorityLevel)
    {
        if (_position >= _tokens.Count)
        {
            return null;
        }

        ISyntaxNode? syntaxNode = null;

        syntaxNode ??= ParseBinaryExpression(priorityLevel);
        syntaxNode ??= ParseUnaryExpression(priorityLevel);
        syntaxNode ??= ParseGrouppedExpression();
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

    private ISyntaxNode? ParseUnaryExpression(int priorityLevel)
    {
        if (!_hasUnaryOperator)
        {
            return null;
        }
        if (priorityLevel > _maxOperatorPriority)
        {
            return null;
        }

        if (Current is not IUnaryOperatorToken operandToken)
        {
            return null;
        }
        if (operandToken.UnaryPriority < priorityLevel)
        {
            return null;
        }

        LeafNode? @operator = ParseLeaf();
        if (@operator is null)
        {
            return null;
        }

        var operand = ParseNext(priorityLevel);

        if (operand is null)
        {
            _position--;
            return null;
        }

        if (operand is LeafNode leaf && leaf.Token is not IOperandToken)
        {
            _position--;
            return null;
        }

        return new UnaryExpressionNode(@operator, operand);
    }

    private ISyntaxNode? ParseBinaryExpression(int priorityLevel)
    {
        if (!_hasBinaryOperator)
        {
            return null;
        }
        if (priorityLevel > _maxOperatorPriority)
        {
            return null;
        }
        ISyntaxNode? leftOperand = ParseNext(priorityLevel + 1);
        if (leftOperand is null)
        {
            return null;
        }

        if (leftOperand is LeafNode leaf && leaf.Token is not IOperandToken)
        {
            _position--;
            return null;
        }

        if (
            Current is not IBinaryOperatorToken operandToken
            || operandToken.BinaryPriority < priorityLevel
        )
        {
            return leftOperand;
        }

        LeafNode? @operator = ParseLeaf();

        if (@operator is null)
        {
            return leftOperand;
        }

        ISyntaxNode? rightOperand = ParseNext(priorityLevel);

        if (
            rightOperand is null
            || (rightOperand is LeafNode rightLeaf && rightLeaf.Token is not IOperandToken)
        )
        {
            throw new SyntaxException(
                "Unable to parse Binary Expression. Missing operand from the right side."
            );
        }

        return new BinaryExpressionNode(leftOperand, @operator, rightOperand);
    }

    private ISyntaxNode? ParseGrouppedExpression()
    {
        if (Current is not OpenBracketToken open)
        {
            return null;
        }
        var openLeaf = ParseLeaf();
        if (openLeaf is null)
        {
            return null;
        }
        var expression = ParseNext(_minOperatorPriority);

        if (expression is null)
        {
            throw new SyntaxException("Missing expression from group.");
        }

        var closeLeaf = ParseLeaf();
        if (closeLeaf is null || closeLeaf.Token is not CloseBracketToken)
        {
            throw new SyntaxException("Missing close bracket in group expression.");
        }

        return new GrouppedExpressionNode(openLeaf, expression, closeLeaf);
    }
}
