using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter.Utils;

public static class StringUtils
{
    public static string Multiply(this string text, int amount)
    {
        return string.Concat(Enumerable.Repeat(text, amount));
    }
}
