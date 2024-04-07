using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Calculator.State;

public class InterpreterConfig
{
    public bool PrintAstToConsole { get; set; } = false;

    [JsonConverter(typeof(StringEnumConverter))]
    public PrintStacktracesOptions PrintStacktraces { get; set; } = PrintStacktracesOptions.None;
}
