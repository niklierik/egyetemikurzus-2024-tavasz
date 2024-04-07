namespace Calculator.NativeMethods;

// These methods are exported for the calculator, so the user can invoke them
// They are not used in the software
public static class Boolean
{
    private static bool[] ParseBoolArray(params object?[] values)
    {
        bool[] bools = values
            .Select(value =>
            {
                if (value is not bool b)
                {
                    return Bool(value);
                }
                return b;
            })
            .ToArray();
        return bools;
    }

    [VarArgs]
    public static bool And(params object?[] values)
    {
        bool[] bools = ParseBoolArray(values);

        foreach (var val in bools)
        {
            if (!val)
            {
                return false;
            }
        }
        return true;
    }

    [VarArgs]
    public static bool Or(params object?[] values)
    {
        bool[] bools = ParseBoolArray(values);
        foreach (var val in bools)
        {
            if (val)
            {
                return true;
            }
        }
        return false;
    }

    public static bool Not(bool value)
    {
        return !value;
    }

    public static bool Bool(object? value)
    {
        if (value is null)
        {
            return false;
        }
        if (value is double number && number == 0.0)
        {
            return false;
        }
        if (value is string str && string.IsNullOrEmpty(str))
        {
            return false;
        }
        if (value is bool boolean)
        {
            return boolean;
        }
        return true;
    }
}
