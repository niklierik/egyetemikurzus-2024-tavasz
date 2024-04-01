using Calculator.Syntax.AST;

namespace Calculator.Evaluators.ExpressionEvals;

public interface ISubEvaluator
{
    public object? Evaluate(ISyntaxNode arg);
}
