using System.Globalization;

namespace Calculator.IO.Logging;

public class LogTargetProvider : ILogTargetProvider
{
    private readonly DateTime _createdInstanceAt = DateTime.Now;
    private readonly ICollection<ILogTarget> _targets;

    public LogTargetProvider()
    {
        Directory.CreateDirectory("logs");

        var timestamp = _createdInstanceAt.ToString(
            "yyyy_MM_ddTHH_mm_ss",
            CultureInfo.InvariantCulture
        );

        _targets = [new FileLogger(Path.Combine("logs", $"{timestamp}.log"))];
    }

    public ICollection<ILogTarget> GetLogTargets() => _targets;
}
