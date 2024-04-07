using System.Reflection;
using Calculator.Evaluators.ExpressionEvals.BinaryOps;
using Calculator.Syntax;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals;

[EvaluatorFor(typeof(BinaryExpressionNode))]
public class BinaryExpressionEvaluator(IEvaluator evaluator) : ISubEvaluator
{
    private readonly IEvaluator _evaluator = evaluator;

    public async Task<object?> Evaluate(ISyntaxNode arg)
    {
        if (arg is not BinaryExpressionNode binaryExpression)
        {
            throw new EvaluatorException($"Unable to process node {arg} as Binary Expression.");
        }
        ISyntaxNode leftArg = binaryExpression.Left;
        ISyntaxNode rightArg = binaryExpression.Right;

        ISubEvaluator leftEvaluator = _evaluator.GetEvaluatorFor(leftArg);
        ISubEvaluator rightEvaluator = _evaluator.GetEvaluatorFor(rightArg);

        ISyntaxToken operatorSymbol = binaryExpression.Operator.Token;
        if (operatorSymbol is not IBinaryOperatorToken token)
        {
            throw new EvaluatorException(
                $"Found a Binary Expression without the Operator being Binary (it is actually {operatorSymbol.GetType()})."
            );
        }

        IBinaryOperator op = _evaluator.GetBinaryOperator(token);
        BinaryOpAttribute? binaryOpAttribute = op.GetType().GetCustomAttribute<BinaryOpAttribute>();

        if (binaryOpAttribute is null)
        {
            throw new EvaluatorException(
                $"BinaryOperator {op.GetType()} has missing BinaryOp attribute."
            );
        }

        object? leftValue = leftArg;
        if (!binaryOpAttribute.KeepLeftTree)
        {
            leftValue = await leftEvaluator.Evaluate(leftArg);
        }

        object? rightValue = rightArg;
        if (!binaryOpAttribute.KeepRightTree)
        {
            rightValue = await rightEvaluator.Evaluate(rightArg);
        }

        object? result = op.Evaluate(leftValue, rightValue);

        return result;
    }
}
