namespace Calculator.State;

public interface IJsonService
{
    public Task<T?> LoadJsonDocument<T>(string path);

    public Task SaveJsonDocument(string path, object? value);

    public string ToJson(object? value);
    public T? FromJson<T>(string value);
}
