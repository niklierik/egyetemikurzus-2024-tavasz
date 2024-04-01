using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals.BinaryOps;

[BinaryOp(typeof(DivideToken))]
public class DivideOperator : IBinaryOperator
{
    public object Evaluate(object? leftValue, object? rightValue)
    {
        if (leftValue is not double left || rightValue is not double right)
        {
            throw new TypeException("Divide operator requires both operands to be numbers.");
        }

        return left / right;
    }
}
