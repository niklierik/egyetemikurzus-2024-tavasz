using System.Text.Json;
using Calculator.IO.Logging;

namespace Calculator.State;

public class InterpreterConfigLoader(IJsonService jsonService, ILogManager logger)
    : IConfigLoader<InterpreterConfig>
{
    private readonly IJsonService _jsonService = jsonService;
    private readonly ILogManager _logger = logger;

    public async Task<InterpreterConfig> Load(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                await _logger.Debug($"Missing state file, creating new at {path}.");
                return await CreateAndSaveNewState(path);
            }
            return await _jsonService.LoadJsonDocument<InterpreterConfig>(path) ?? new();
        }
        catch (JsonException e)
        {
            await _logger.Error($"Failed to load state from {path}.", e);
            return await CreateAndSaveNewState(path);
        }
        catch (IOException e)
        {
            await _logger.Error($"Failed to load state from {path}.", e);
            return await CreateAndSaveNewState(path);
        }
        catch (Exception e)
        {
            await _logger.Error($"Failed to load state from {path}.", e);
            throw;
        }
    }

    private async Task<InterpreterConfig> CreateAndSaveNewState(string path)
    {
        var state = new InterpreterConfig();
        await Save(path, state);
        return state;
    }

    public async Task Save(string path, InterpreterConfig state)
    {
        try
        {
            await _jsonService.SaveJsonDocument(path, state);
        }
        catch (Exception e)
        {
            await _logger.Error($"Unable to save state to {path}.", e);
            throw;
        }
    }
}
