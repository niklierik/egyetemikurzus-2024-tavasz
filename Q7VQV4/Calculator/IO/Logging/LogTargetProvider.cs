using System.Globalization;

namespace Calculator.IO.Logging;

public class LogTargetProvider : ILogTargetProvider
{
    private readonly DateTime _createdInstanceAt = DateTime.Now;

    public ICollection<ILogTarget> GetLogTargets()
    {
        var timestamp = _createdInstanceAt.ToString(
            "yyyy_MM_ddTHH_mm_ss",
            CultureInfo.InvariantCulture
        );

        return [new FileLogger($"{timestamp}.log")];
    }
}
