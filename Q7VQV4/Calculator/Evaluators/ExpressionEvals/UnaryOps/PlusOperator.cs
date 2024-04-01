using Calculator.Evaluators.Exceptions;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals.UnaryOps;

[UnaryOp(typeof(AddToken))]
public class PlusOperator : IUnaryOperator
{
    public object? Evaluate(object? value)
    {
        if (value is not double number)
        {
            throw new TypeException(
                $"Plus operator requires operand to be a number. (Tried +{value ?? "null"})"
            );
        }

        return +number;
    }
}
