using Shell.Infrastructure;

namespace Shell.UserInterface
{
    internal interface ICommandProvider
    {
        IShellCommand[] Commands { get; }
    }
}