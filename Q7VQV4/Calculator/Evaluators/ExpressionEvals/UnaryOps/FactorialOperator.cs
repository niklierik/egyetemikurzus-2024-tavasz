using Calculator.Evaluators.Exceptions;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals.UnaryOps;

[UnaryOp(typeof(FactorialToken))]
public class FactorialOperator : IUnaryOperator
{
    public object? Evaluate(object? value)
    {
        if (value is not double number || number != Math.Floor(number))
        {
            throw new TypeException(
                $"Factorial operator requires an integer operand. (Current: {value}!)"
            );
        }
        double prod = 1;
        for (long i = 1; i <= number; i++)
        {
            prod *= i;
        }
        return prod;
    }
}
