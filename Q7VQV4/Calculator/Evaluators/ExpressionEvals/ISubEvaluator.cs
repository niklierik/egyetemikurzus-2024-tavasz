using Calculator.Syntax.AST;

namespace Calculator.Evaluators.ExpressionEvals;

public interface ISubEvaluator
{
    public Task<object?> Evaluate(ISyntaxNode arg);
}
