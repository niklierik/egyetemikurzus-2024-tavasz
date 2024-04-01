using Calculator.Syntax.AST;

namespace Calculator.Evaluators.ExpressionEvals;

[EvaluatorFor(typeof(GrouppedExpressionNode))]
public class GrouppedExpressionEvaluator(IEvaluator evaluator) : ISubEvaluator
{
    private readonly IEvaluator _evaluator = evaluator;

    public object? Evaluate(ISyntaxNode arg)
    {
        if (arg is not GrouppedExpressionNode grouppedExpression)
        {
            throw new EvaluatorException($"Unable to process {arg} as Groupped Expression.");
        }
        var evaluator = _evaluator.GetEvaluatorFor(grouppedExpression.Expression);
        return evaluator.Evaluate(grouppedExpression.Expression);
    }
}
