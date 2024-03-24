namespace Shell.Infrastructure
{
    internal interface IShellCommand
    {
        string Name { get; }
        void Execute(IHost host, string[] args);
    }
}
