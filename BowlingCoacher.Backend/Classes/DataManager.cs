using BowlingCoacher.Backend.DataModels;
using ErrorLogging;

namespace BowlingCoacher.Backend.Classes;

//  NOTE:   10th Frame only counts as a strike if its the first ball, only count once, regradless of the number of actual strikes.
internal class DataManager {
    private GameStatistics _combinedStatistics;

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
    public void SetStatisticalValues (List<GameStatistics> gameStatistics){
        if (gameStatistics == null){
            LoggingManager.Instance.LogWarning("Failed to set the Statistical values, no data was provided.");
            return;
        }
        
        try {
            _combinedStatistics = new GameStatistics();
        
            foreach (var stat in gameStatistics){
                _combinedStatistics.Score += stat.Score;
                _combinedStatistics.Games += stat.Games;
                _combinedStatistics.Strikes += stat.Strikes;
                _combinedStatistics.Spares += stat.Spares;
                _combinedStatistics.Opens += stat.Opens;
            }
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem trying to set the statistical valeues.");
        }
    }

    //  This method is used dto get the statistical values and returns them to the calling class as a float.
    //  If an invalid category is provided it returns -1 to indicate an error was encountered.
    public float GetStatisticsValue (StatCategories statCategories){
        return statCategories switch {
            StatCategories.Strikes => CalculatePercentage(_combinedStatistics.Strikes, CalculateNumberOfAttempts()),
            StatCategories.Spares => CalculatePercentage(_combinedStatistics.Spares, CalculateNumberOfAttempts()),
            StatCategories.OpenFrames => CalculatePercentage(_combinedStatistics.Opens, CalculateNumberOfAttempts()),
            StatCategories.Average => CalculateAverage(),
            _ => -1f,
        };
    }

    //  This method is used to calculate the average of the data that is supplied. If this somehow breaks, then it will return
    //  -1 indicating that an error was encountered.
    private float CalculateAverage (){
        try {
            float average = (float)Math.Round(_combinedStatistics.Score / _combinedStatistics.Games, 2);
            LoggingManager.Instance.LogInformation($"The Average was calculated as being {average}, for the {_combinedStatistics.Games} games bowled. Based off the combined score of {_combinedStatistics.Score}");

            return average;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem while attempting to calculate the average.");
            return -1.0f;
        }
    }

    //  This method is used to calculate the number of attempts taken. If it encounteres a problem then it returns -1 indicating that an error was encountered.
    private float CalculateNumberOfAttempts (){
        try {
            float numberOfAttempts = _combinedStatistics.Strikes + _combinedStatistics.Spares + _combinedStatistics.Opens;
            LoggingManager.Instance.LogInformation($"Successfully calculated the number of attempts as: {numberOfAttempts}.");

            return numberOfAttempts;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to calculate the number of attempts something went wrong.");
            return -1.0f;
        }
    }
}