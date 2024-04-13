using Calculator.Evaluators.Exceptions;
using Calculator.Interpreters;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals.BinaryOps;

[BinaryOp(typeof(AssignmentToken), keepLeftTree: true)]
public class AssignmentOperator(IInterpreter interpreter) : IBinaryOperator
{
    private readonly IInterpreter _interpreter = interpreter;

    public object? Evaluate(object? leftValue, object? rightValue)
    {
        if (leftValue is not LeafNode leafNode || leafNode.Token is not IdentifierToken id)
        {
            throw new SyntaxException(
                "Assignment (=) operator requires an identifier on the left side."
            );
        }

        var variableName = id.Content;

        if (_interpreter.State.Consts.ContainsKey(variableName))
        {
            throw new ConstantOverwriteException(
                $"Unable to overwrite constant variable '{variableName}'."
            );
        }
        _interpreter.State.Variables[variableName] = rightValue;

        return rightValue;
    }
}
