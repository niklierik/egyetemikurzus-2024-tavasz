using System.Collections.Immutable;
using Calculator.IO.Logging;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Syntax.Parser;

// TODO: remove state
public class Parser : IParser
{
    private IReadOnlyList<ISyntaxToken> _tokens = [];
    private int _position = 0;
    private int _minOperatorPriority = int.MaxValue;
    private int _maxOperatorPriority = int.MinValue;
    private bool _hasBinaryOperator = false;
    private bool _hasUnaryOperator = false;

    private ISyntaxToken? Current => TokenAt(_position);

    private ISyntaxToken? TokenAt(int position)
    {
        if (position >= _tokens.Count)
        {
            return null;
        }
        return _tokens[position];
    }

    private ISyntaxToken? TokenRelative(int offset) => TokenAt(_position + offset);

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
            _tokens = [];
            _minOperatorPriority = int.MaxValue;
            _maxOperatorPriority = int.MinValue;
            _hasBinaryOperator = false;
            _hasUnaryOperator = false;
        }
    }

    private void SetOperatorInfos()
    {
        var binaryOperatorsByPriority = _tokens
            .Where(token => token is IBinaryOperatorToken)
            .Cast<IBinaryOperatorToken>()
            .Select(@operator => @operator.BinaryPriority)
            .ToImmutableList();

        _hasBinaryOperator = !binaryOperatorsByPriority.IsEmpty;

        if (_hasBinaryOperator)
        {
            _minOperatorPriority = binaryOperatorsByPriority.Min();
            _maxOperatorPriority = binaryOperatorsByPriority.Max();
        }

        var unaryOperatorsByPriority = _tokens
            .Where(token => token is IUnaryOperatorToken)
            .Cast<IUnaryOperatorToken>()
            .Select(@operator => @operator.UnaryPriority)
            .ToImmutableList();

        _hasUnaryOperator = !unaryOperatorsByPriority.IsEmpty;

        if (_hasUnaryOperator)
        {
            _minOperatorPriority = Math.Min(unaryOperatorsByPriority.Min(), _minOperatorPriority);
            _maxOperatorPriority = Math.Max(unaryOperatorsByPriority.Max(), _maxOperatorPriority);
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

        syntaxNode ??= ParseImportStatement();
        syntaxNode ??= ParseBinaryExpression(priorityLevel);
        syntaxNode ??= ParsePrefixedUnaryExpression(priorityLevel);
        syntaxNode ??= ParseGrouppedExpression();
        syntaxNode ??= ParseMethodInvication();
        syntaxNode ??= ParseLeaf();
        syntaxNode = ParsePostfixUnaryExpression(syntaxNode);
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

    private ISyntaxNode? ParseImportStatement()
    {
        if (Current is not ImportToken)
        {
            return null;
        }
        var import = ParseLeaf();

        if (import is null)
        {
            throw new SyntaxException(
                "Tried to parse an import statement, but could not parse the 'import' keyword."
            );
        }

        var args = new List<LeafNode>();

        while (Current is not FromToken)
        {
            if (!(Current is MultiplyToken or IdentifierToken))
            {
                throw new SyntaxException(
                    "Import statement must be followed with any number and combination of Identifier, a '*' and the last symbol must be a 'from' keyword."
                );
            }
            var leaf = ParseLeaf();
            if (leaf is null)
            {
                throw new SyntaxException(
                    $"Failed to parse topken {Current} as leaf node for import statement."
                );
            }
            args.Add(leaf);
        }

        var from = ParseLeaf();
        if (from is null)
        {
            throw new SyntaxException(
                "Tried to parse an import statement, but could not parse the 'from' keyword."
            );
        }
        if (Current is not IdentifierToken)
        {
            throw new SyntaxException(
                "'import ... from' must be followed with an identifier containing a C# class."
            );
        }
        var csharpClass = ParseLeaf();

        if (csharpClass is null)
        {
            throw new SyntaxException(
                "'import ... from' must be followed with an identifier containing a C# class."
            );
        }

        return new ImportExpressionNode(import, args, from, csharpClass);
    }

    private ISyntaxNode? ParsePrefixedUnaryExpression(int priorityLevel)
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

    private ISyntaxNode? ParsePostfixUnaryExpression(ISyntaxNode? node)
    {
        if (node is null)
        {
            return null;
        }
        if (Current is not IUnaryOperatorToken operatorToken || !operatorToken.PostOperator)
        {
            return node;
        }
        var @operator = ParseLeaf();
        if (@operator is null)
        {
            return node;
        }
        return new UnaryExpressionNode(@operator, node);
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

    private ISyntaxNode? ParseMethodInvication()
    {
        if (Current is not IdentifierToken || TokenRelative(1) is not OpenBracketToken)
        {
            return null;
        }

        var methodName = ParseLeaf();
        if (methodName is null)
        {
            return null;
        }
        var openBracket = ParseLeaf();
        if (openBracket is null)
        {
            return null;
        }
        if (Current is CloseBracketToken)
        {
            var closeBracket = ParseLeaf();
            if (closeBracket is null)
            {
                return null;
            }
            return new MethodInvocationNode(methodName, openBracket, [], closeBracket);
        }
        List<ISyntaxNode> args = [];
        while (true)
        {
            var node = ParseNext(_minOperatorPriority);
            if (node is null)
            {
                break;
            }
            args.Add(node);

            if (Current is not CommaToken)
            {
                break;
            }

            ParseLeaf();
        }

        var close = ParseLeaf();
        if (close is null || close.Token is not CloseBracketToken)
        {
            throw new SyntaxException("Missing Close Bracket for method invocation.");
        }
        return new MethodInvocationNode(methodName, openBracket, args, close);
    }
}
