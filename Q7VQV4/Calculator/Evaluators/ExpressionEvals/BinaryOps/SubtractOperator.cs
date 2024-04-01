using Calculator.Evaluators.Exceptions;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals.BinaryOps;

[BinaryOp(typeof(SubtractToken))]
public class SubtractOperator : IBinaryOperator
{
    public object Evaluate(object? leftValue, object? rightValue)
    {
        if (leftValue is not double left || rightValue is not double right)
        {
            throw new TypeException(
                $"Subtract operator requires both operands to be numbers. (Tried: {leftValue ?? "null"} - {rightValue ?? "null"})"
            );
        }

        return left - right;
    }
}
