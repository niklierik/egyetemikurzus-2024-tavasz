using Shell.UserInterface;

namespace Shell
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ui = new Ui(new ReflectionCommandLoader(), new Host());
            ui.Run();
        }
    }
}
