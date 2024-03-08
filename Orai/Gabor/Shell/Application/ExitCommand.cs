using Shell.Infrastructure;

namespace Shell.Application
{
    internal class ExitCommand : IShellCommand
    {
        public string Name => "exit";

        //public string Name 
        //{
        //    get { return "exit"; }
        //}

        public void Execute(IHost host, string[] args)
        {
            host.Exit();
        }
    }
}
