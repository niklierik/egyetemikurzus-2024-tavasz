namespace Calculator.State;

public interface IConfigLoader<TConfig>
{
    Task<TConfig> Load(string path);
    Task Save(string path, TConfig config);
}
