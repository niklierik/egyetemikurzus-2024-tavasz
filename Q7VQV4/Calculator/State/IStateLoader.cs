namespace Calculator.State;

public interface IStateLoader<TState>
{
    Task<TState> LoadState(string path);
}
