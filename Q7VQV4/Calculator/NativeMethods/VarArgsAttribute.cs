namespace Calculator.NativeMethods;

/// <summary>
/// Mark that a method waits a varrying number of arguments
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class VarArgsAttribute : Attribute { }
