using Calculator.Evaluators.ExpressionEvals;
using Calculator.Evaluators.ExpressionEvals.BinaryOps;
using Calculator.Evaluators.ExpressionEvals.UnaryOps;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators;

public interface IEvaluator
{
    public Task<object?> Evaluate(RootNode node);

    public ISubEvaluator GetEvaluatorFor(ISyntaxNode node);

    public IBinaryOperator GetBinaryOperator<T>() => GetBinaryOperator(typeof(T));
    public IBinaryOperator GetBinaryOperator<T>(T token)
        where T : IBinaryOperatorToken => GetBinaryOperator(token.GetType());
    public IBinaryOperator GetBinaryOperator(Type t);
    public IUnaryOperator GetUnaryOperator<T>(T token)
        where T : IUnaryOperatorToken => GetUnaryOperator(token.GetType());
    public IUnaryOperator GetUnaryOperator<T>() => GetUnaryOperator(typeof(T));
    public IUnaryOperator GetUnaryOperator(Type t);
}
