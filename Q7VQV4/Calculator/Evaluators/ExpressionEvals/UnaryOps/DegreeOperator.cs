using Calculator.Evaluators.Exceptions;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals.UnaryOps;

[UnaryOp(typeof(DegreeToken))]
public class DegreeOperator : IUnaryOperator
{
    public object? Evaluate(object? value)
    {
        if (value is not double number)
        {
            throw new TypeException(
                $"Degree Operator requires the operand to be a number (Currently: {value}°)"
            );
        }

        return number * Math.PI / 180;
    }
}
