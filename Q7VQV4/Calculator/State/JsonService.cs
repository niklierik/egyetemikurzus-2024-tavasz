using Newtonsoft.Json;

namespace Calculator.State;

public class JsonService : IJsonService
{
    public async Task SaveJsonDocument(string path, object? value)
    {
        var json = ToJson(value);
        using var writer = new StreamWriter(path);
        await writer.WriteLineAsync(json);
    }

    public async Task<T?> LoadJsonDocument<T>(string path)
    {
        string json;
        using (var reader = new StreamReader(path))
        {
            json = await reader.ReadToEndAsync();
        }
        return FromJson<T>(json);
    }

    public string ToJson(object? value)
    {
        return JsonConvert.SerializeObject(
            value,
            Formatting.Indented,
            new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }
        );
    }

    public T? FromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}
