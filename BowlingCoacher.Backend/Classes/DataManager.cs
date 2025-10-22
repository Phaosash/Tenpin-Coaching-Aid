using BowlingCoacher.Backend.DataModels;
using ErrorLogging;

namespace BowlingCoacher.Backend.Classes;

//  NOTE:   10th Frame only counts as a strike if its the first ball, only count once, regradless of the number of actual strikes.
internal class DataManager {
    //  This method is used to calculate the percentage based on teh supplied data. If at any point it encounters a problem,
    //  or if bad data is provided it will return -1, indicating that an error was encountered.
    private static float CalculatePercentage (float part, float whole){
        if (whole <= 0){
            LoggingManager.Instance.LogWarning("Failed to calculate the most recent percentage, the whole value was invalid.");
            return -1.0f;
        }

        try {
            float percentage = part / whole * 100;
            LoggingManager.Instance.LogInformation($"Successfully calculated the percentage as: {percentage} %");

            return percentage;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem trying to calculate the percentage.");
            
            return -1.0f;
        }
    }

    //  This method is used to to set/initialise the values for the statistical data. It does this by iterating through the
    //  supplied list of statistical objects, and then stores the combined values into a new object for manipulation else where.
    public static GameStatistics SetStatisticalValues (List<GameStatistics> gameStatistics){
        if (gameStatistics == null){
            LoggingManager.Instance.LogWarning("Failed to set the Statistical values, no data was provided.");
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
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem trying to set the statistical valeues.");
            return new();
        }
    }

    //  This method is used dto get the statistical values and returns them to the calling class as a float.
    //  If an invalid category is provided it returns -1 to indicate an error was encountered.
    public static float GetStatisticsValue (StatCategories statCategories, GameStatistics statistics){
        return statCategories switch {
            StatCategories.Strikes => CalculatePercentage(statistics.Strikes, CalculateNumberOfAttempts(statistics)),
            StatCategories.Spares => CalculatePercentage(statistics.Spares, CalculateNumberOfAttempts(statistics)),
            StatCategories.OpenFrames => CalculatePercentage(statistics.Opens, CalculateNumberOfAttempts(statistics)),
            StatCategories.Average => CalculateAverage(statistics),
            _ => -1f,
        };
    }

    //  This method is used to calculate the average of the data that is supplied. If this somehow breaks, then it will return
    //  -1 indicating that an error was encountered.
    private static float CalculateAverage (GameStatistics statistics){
        try {
            float average = (float)Math.Round(statistics.Score / statistics.Games, 2);
            LoggingManager.Instance.LogInformation($"The Average was calculated as being {average}, for the {statistics.Games} games bowled. Based off the combined score of {statistics.Score}");

            return average;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem while attempting to calculate the average.");
            return -1.0f;
        }
    }

    //  This method is used to calculate the number of attempts taken. If it encounteres a problem then it returns -1 indicating that an error was encountered.
    private static float CalculateNumberOfAttempts (GameStatistics statistics){
        try {
            float numberOfAttempts = statistics.Strikes + statistics.Spares + statistics.Opens;
            LoggingManager.Instance.LogInformation($"Successfully calculated the number of attempts as: {numberOfAttempts}.");

            return numberOfAttempts;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to calculate the number of attempts something went wrong.");
            return -1.0f;
        }
    }
}