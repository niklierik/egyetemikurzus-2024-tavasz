using Newtonsoft.Json;

namespace Calculator.State;

public class JsonService : IJsonService
{
    public async Task SaveJsonDocument(string path, object? value)
    {
        var json = JsonConvert.SerializeObject(
            value,
            Formatting.Indented,
            new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
        );
        using var writer = new StreamWriter(path);
        await writer.WriteLineAsync(json);
    }

    public async Task<T?> LoadJsonDocument<T>(string path)
    {
        string content;
        using (var reader = new StreamReader(path))
        {
            content = await reader.ReadToEndAsync();
        }
        return JsonConvert.DeserializeObject<T>(content);
    }
}
