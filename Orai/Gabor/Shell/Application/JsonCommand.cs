using Shell.Infrastructure;

namespace Shell.Application;

internal class JsonCommand : IShellCommand
{
    public string Name => "json";

    public void Execute(IHost host, string[] args)
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                   "test.json");

        using (var stream = File.Create(path))
        {
            try
            {
                var serializer = new DagattMacskaSerializer();
                serializer.SerializeToJson(stream, new Domain.DagattMacska
                {
                    Name = "cirmi"
                });
            }
            catch (IOException ex)
            {
                host.WriteLine(ex.Message);
            }
        }
    }
}