using System.Text.Json;

namespace Calculator.State;

public class JsonService : IJsonService
{
    public async Task SaveJsonDocument(string path, object? value)
    {
        using var fileStream = new FileStream(path, FileMode.Create);
        await JsonSerializer.SerializeAsync(fileStream, value);
    }

    public async Task<T?> LoadJsonDocument<T>(string path)
    {
        using var fileStream = new FileStream(path, FileMode.Open);
        return await JsonSerializer.DeserializeAsync<T>(fileStream);
    }
}
