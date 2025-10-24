using BowlingCoacher.Shared.DTO;
using ErrorLogging;

namespace BowlingCoacher.Backend.Classes;

internal class DataProcessor {
    public static float CalculatePercentage (float part, float total){
        try {
            float percentage = part / total * 100.0f;
            LoggingManager.Instance.LogInformation($"Successfully calculated the percentage as: {percentage}");
            return percentage;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to calculate the percentage.");
            return -1.0f;
        }
    }

    public static GameStatistics GetStatisticsObjectValue (List<GameStatistics> gameStatistics){
        if (gameStatistics == null){
            LoggingManager.Instance.LogWarning("Failed to to consolidate the statistics object, no data was supplied.");
            return new();
        }

        try {
            GameStatistics statistics = new();

            foreach (var stat in gameStatistics){
                statistics.Score += stat.Score;
                statistics.Games += stat.Games;
                statistics.Strikes += stat.Strikes;
                statistics.Spares += stat.Spares;
                statistics.Opens += stat.Opens;
            }

            LoggingManager.Instance.LogInformation("Successfully created new statistical data values.");
            return statistics;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to create the consolidated statistics object.");
            return new();
        }
    }

    public static float CalculateAverage (float score, float numberOfGames){
        return (float)Math.Round(score / numberOfGames, 2);
    }
}