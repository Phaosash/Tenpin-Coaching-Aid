using BowlingCoacher.Shared.DTO;
using ErrorLogging;

namespace BowlingCoacher.Backend.Classes;

internal class DataManager {
    private GameStatistics _totalGameStatistics = new();
    private GameStatistics _recentGameStatistics = new();
    private readonly List<GameStatistics> _statisticsList = [];
    private readonly ApiClient _client;
    private static readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:7132") };

    public DataManager (){
        _client = new ApiClient(_httpClient);
    }

    public static async Task LoadDataAsync (DataManager manager){
        await manager.InitialiseDataAsync();
    }

    private async Task InitialiseDataAsync (){
        try {
            var statsFromApi = await _client.GetAllAsync();

            _statisticsList.Clear();

            if (statsFromApi is null || !statsFromApi.Any()){
                LoggingManager.Instance.LogWarning("No existing statistics found from API.");

                GameStatistics game = new(){
                    Score = 0.0f,
                    Games = 0.0f,
                    Strikes = 0.0f,
                    Spares = 0.0f,
                    Opens = 0.0f,
                };
            
                _statisticsList.Add(game);
                _totalGameStatistics = CreateStatisticsObject(_statisticsList);
                _recentGameStatistics = _totalGameStatistics;
            }
            else {
                foreach (var stat in statsFromApi){
                    _statisticsList.Add(stat);
                }

                _recentGameStatistics = _statisticsList.OrderByDescending(s => s.Id).First();
                _totalGameStatistics = CreateStatisticsObject(_statisticsList);
            }
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "");
        }
    }

    public GameStatistics GetGameStatisticsValues (bool useRecent){
        if (useRecent){
            return _recentGameStatistics;
        } else {
            return _totalGameStatistics;
        }
    }

    public float CalculateAverage (bool useRecent){
        if (useRecent){
            return GetAverage(_recentGameStatistics.Score, _recentGameStatistics.Games);
        } else {
            return GetAverage(_totalGameStatistics.Score, _totalGameStatistics.Games);
        }
    }

    public float CalculateStrikePercentage (bool useRecent){
        if (useRecent){
            return GetPercentage(_recentGameStatistics.Strikes, CalculateNumberOfAttempts(_recentGameStatistics));
        } else {
            return GetPercentage(_totalGameStatistics.Strikes, CalculateNumberOfAttempts(_totalGameStatistics));
        }
    }
    
    public float CalculateSparePercentage (bool useRecent){
        if (useRecent){
            return GetPercentage(_recentGameStatistics.Spares, CalculateNumberOfAttempts(_recentGameStatistics));
        } else {
            return GetPercentage(_totalGameStatistics.Spares, CalculateNumberOfAttempts(_totalGameStatistics));
        }
    }

    public float CalculateOpenPercentage (bool useRecent){
        if (useRecent){
            return GetPercentage(_recentGameStatistics.Opens, CalculateNumberOfAttempts(_recentGameStatistics));
        } else {
            return GetPercentage(_totalGameStatistics.Opens, CalculateNumberOfAttempts(_totalGameStatistics));
        }
    }

    private static float GetPercentage (float part, float total){
        return DataProcessor.CalculatePercentage(part, total);
    }
    
    private static GameStatistics CreateStatisticsObject (List<GameStatistics> gameStatistics){
        return DataProcessor.GetStatisticsObjectValue(gameStatistics);
    }

    private static float GetAverage (float score, float numGames){
        return DataProcessor.CalculateAverage(score, numGames);
    }

    //  This method is used to calculate the number of attempts taken.
    private static float CalculateNumberOfAttempts (GameStatistics statistics){
        try {
            return statistics.Strikes + statistics.Spares + statistics.Opens;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to calculate the number of attempts something went wrong.");
            return -1.0f;
        }
    }
}