namespace Calculator.State;

public interface IStateLoader<TState>
{
    Task<TState> LoadState(string path);
    Task SaveState(string path, TState state);
}
