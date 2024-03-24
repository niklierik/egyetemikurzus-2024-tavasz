using Shell.Infrastructure;

namespace Shell.UserInterface
{
    internal class Host : IHost
    {
        public void Exit()
            => Environment.Exit(0);

        public string ReadLine()
            => Console.ReadLine() ?? throw new InvalidOperationException();

        public void WriteLine(string message)
            => Console.WriteLine(message);
    }
}
