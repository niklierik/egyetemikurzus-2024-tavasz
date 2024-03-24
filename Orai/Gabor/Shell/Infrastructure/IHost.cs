using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Infrastructure
{
    /// <summary>
    /// A futtató alkalmazás interfésze
    /// Olyan funkciókat definiál
    /// Amihez egy IShellCommand hozzáférhet
    /// </summary>
    internal interface IHost
    {
        string ReadLine();
        void WriteLine(string message);
        void Write(string message)
            => Console.Write(message);
        void Exit();
    }
}
