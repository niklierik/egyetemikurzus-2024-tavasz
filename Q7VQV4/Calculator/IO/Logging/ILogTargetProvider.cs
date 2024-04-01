namespace Calculator.IO.Logging;

public interface ILogTargetProvider
{
    ICollection<ILogTarget> GetLogTargets();
}
