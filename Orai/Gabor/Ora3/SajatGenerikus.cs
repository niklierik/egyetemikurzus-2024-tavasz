using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ora3;

internal class SajatGenerikus<T>
    where T : class, IEquatable<T>
{
    public void DoSomething(T instance)
    {
    }
}

internal class NemGenerikus
{
    public void DoSomething<T>(T instance)
    {

    }
}
