using BowlingCoacher.Backend.DataModels;

namespace BowlingCoacher.Backend.Classes;

internal class DataManager {
    private GameStatistics _combinedStatistics = new();

    private static float CalculatePercentage (float part, float whole){
        return part / whole * 100f;
    }

    public void SetStatisticalValues (List<GameStatistics> gameStatistics){
        foreach (var stat in gameStatistics){
            _combinedStatistics.Score += stat.Score;
            _combinedStatistics.Games += stat.Games;
            _combinedStatistics.Strikes += stat.Strikes;
            _combinedStatistics.Spares += stat.Spares;
            _combinedStatistics.Opens += stat.Opens;
        }
    }

    public float GetStatisticsValue (StatCategories statCategories){
        return statCategories switch {
            StatCategories.Strikes => CalculatePercentage(_combinedStatistics.Strikes, _combinedStatistics.TotalAttempts),
            StatCategories.Spares => CalculatePercentage(_combinedStatistics.Spares, _combinedStatistics.TotalAttempts),
            StatCategories.OpenFrames => CalculatePercentage(_combinedStatistics.Opens, _combinedStatistics.TotalAttempts),
            StatCategories.Average => _combinedStatistics.AverageScore,
            _ => -1,
        };
    }
}