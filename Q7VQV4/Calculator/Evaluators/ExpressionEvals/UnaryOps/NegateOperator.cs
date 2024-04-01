using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals.UnaryOps;

[UnaryOp(typeof(SubtractToken))]
public class NegateOperator : IUnaryOperator
{
    public object Evaluate(object? value)
    {
        if (value is not double number)
        {
            throw new TypeException("Negate operator requires operand to be a number.");
        }

        return -number;
    }
}
