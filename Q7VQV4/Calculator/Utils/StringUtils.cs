namespace Calculator.Utils;

public static class StringUtils
{
    // https://stackoverflow.com/questions/21755757/first-character-of-string-lowercase-c-sharp
    public static string Decapitalize(this string str)
    {
        if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
            return str.Length == 1
                ? char.ToLower(str[0]).ToString()
                : char.ToLower(str[0]) + str[1..];

        return str;
    }

    public static string Capitalize(this string str)
    {
        if (!string.IsNullOrEmpty(str) && char.IsLower(str[0]))
            return str.Length == 1
                ? char.ToUpper(str[0]).ToString()
                : char.ToUpper(str[0]) + str[1..];

        return str;
    }

    public static string Stringify(this object? o)
    {
        return o?.ToString() ?? "null";
    }
}
