using BowlingCoacher.Backend.DataModels;
using BowlingCoacher.Shared.DTO;
using ErrorLogging;

namespace BowlingCoacher.Backend.Classes;

public class ApplicationManager {
    private readonly List<GameStatistics> _gameStatistics;
    private GameStatistics _combinedStatistics;
    private GameStatistics _recentStatistics;
    private readonly ApiClient _client;
    private static readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:7132") };

    public ApplicationManager (){
        _combinedStatistics = new();
        _recentStatistics = new();
        _gameStatistics = [];
        _client = new ApiClient(_httpClient);
        _ = InitialiseDataAsync();
    }

    public static async Task CreateAsync (ApplicationManager manager){
        await manager.InitialiseDataAsync();
    }

    private async Task InitialiseDataAsync (){
        try {
            var statsFromApi = await _client.GetAllAsync();
            
            if (statsFromApi is null || !statsFromApi.Any()){
                LoggingManager.Instance.LogWarning("No existing statistics found from API.");
                return;
            }

            _gameStatistics.Clear();

            foreach (var stat in statsFromApi){
                _gameStatistics.Add(stat);
            }

            _recentStatistics = _gameStatistics.OrderByDescending(s => s.Id).First();
            _combinedStatistics = DataManager.SetStatisticalValues(_gameStatistics);
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to initialise data from API.");
        }
    }

    //  This method is used to tell the data manager to initialise the statistical data based off the information
    //  supplied throught the Tuple. A Tuple was used as it allows the front end to pass dumb data to the backend
    //  without giving it direct context to the specific DTO's that exist in the backend.
    public async Task AddNewStatisticalDataAsync (Tuple<float, float, float, float, float> statsObj){
        if (statsObj == null){
            LoggingManager.Instance.LogWarning("Failed to add the new data values, no data object was found.");
            return;
        }

        try {
            GameStatistics stats = new(){
                Score = statsObj.Item1,
                Games = statsObj.Item2,
                Strikes = statsObj.Item3,
                Spares = statsObj.Item4,
                Opens = statsObj.Item5
            };

            var created = await _client.CreateAsync(stats);
            if (created == null){
                LoggingManager.Instance.LogWarning("API failed to create the new game statistics entry.");
                return;
            }

            _gameStatistics.Add(created);
            _recentStatistics = created;
            _combinedStatistics = DataManager.SetStatisticalValues(_gameStatistics);

            LoggingManager.Instance.LogInformation($"Added new stats: ID={created.Id}, Score={created.Score}");
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem while adding the new statistical data.");
        }
    }

    //  This method is used to return the current strike percentage to the front end.
    public float GetOverallStrikePercentage (){
        return GetDataValue(StatCategories.Strikes, _combinedStatistics);
    }

    //  This method is used to return the current spare percentage to the front end.
    public float GetOverallSparePercentage (){
        return GetDataValue(StatCategories.Spares, _combinedStatistics);
    }

    //  This method is used to return the current percentage of open frames to the front end.
    public float GetOverallOpenPercentage (){
        return GetDataValue(StatCategories.OpenFrames, _combinedStatistics);
    }

    //  This method is used to return the current average score to the front end.
    public float GetOverallAverage (){
        return GetDataValue(StatCategories.Average, _combinedStatistics);
    }

    //  This method is used to get the most recent average to the front end.
    public float GetRecentAverage (){
        return GetDataValue(StatCategories.Average, _recentStatistics);
    }

    public float GetRecentStrikePercentage (){
        return GetDataValue(StatCategories.Strikes, _recentStatistics);
    }

    public float GetRecentSparePercentage (){
        return GetDataValue(StatCategories.Spares, _recentStatistics);
    }

    public float GetRecentOpenPercentage (){
        return GetDataValue(StatCategories.OpenFrames, _recentStatistics);
    }

    //  This method is used to return the requested numeric value to the calling method. If the returned value is
    //  -1 it indicates that an error was encounterd and the correct value wasn't returned from the data manager.
    //  In which case a value of zero will be returned to the front end to display.
    private static float GetDataValue (StatCategories statCategories, GameStatistics gameStatistics){
        try {
            float dataValue = DataManager.GetStatisticsValue(statCategories, gameStatistics);

            if (dataValue == -1.0f){
                LoggingManager.Instance.LogWarning("The returned data value indicated that an error was encountered.");
            } else {
                LoggingManager.Instance.LogInformation($"The value of the requested data is: {dataValue}");
            }

            return dataValue;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem, while attempting to get the data value.");
            return -1.0f;
        }
    }
}