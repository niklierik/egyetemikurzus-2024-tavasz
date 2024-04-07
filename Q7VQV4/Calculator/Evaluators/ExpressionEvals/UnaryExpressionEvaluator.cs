using Calculator.Evaluators.ExpressionEvals.UnaryOps;
using Calculator.Syntax;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals;

[EvaluatorFor(typeof(UnaryExpressionNode))]
public class UnaryExpressionEvaluator(IEvaluator evaluator) : ISubEvaluator
{
    private readonly IEvaluator _evaluator = evaluator;

    public async Task<object?> Evaluate(ISyntaxNode arg)
    {
        if (arg is not UnaryExpressionNode unaryExpression)
        {
            throw new EvaluatorException($"Unable to process node {arg} as Unary Expression.");
        }
        ISyntaxNode operandArg = unaryExpression.Operand;

        ISubEvaluator operandEvaluator = _evaluator.GetEvaluatorFor(operandArg);

        object? operandValue = await operandEvaluator.Evaluate(operandArg);

        ISyntaxToken operatorSymbol = unaryExpression.Operator.Token;
        if (operatorSymbol is not IUnaryOperatorToken token)
        {
            throw new EvaluatorException(
                $"Found a Unary Expression without the Operator being Unary (it is actually {operatorSymbol.GetType()})."
            );
        }

        IUnaryOperator op = _evaluator.GetUnaryOperator(token);

        object? result = op.Evaluate(operandValue);

        return result;
    }
}
