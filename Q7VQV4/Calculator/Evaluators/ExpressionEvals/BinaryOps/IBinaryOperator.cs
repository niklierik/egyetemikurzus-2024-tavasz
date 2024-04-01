namespace Calculator.Evaluators.ExpressionEvals.BinaryOps;

public interface IBinaryOperator
{
    public object? Evaluate(object? leftValue, object? rightValue);
}
