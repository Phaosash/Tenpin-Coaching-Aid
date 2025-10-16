using BowlingCoacher.Backend.DataModels;
using ErrorLogging;

namespace BowlingCoacher.Backend.Classes;

//  NOTE:   10th Frame only counts as a strike if its the first ball, only count once, regradless of the number of actual strikes.
internal class DataManager {
    private GameStatistics _combinedStatistics;

    private static float CalculatePercentage (float part, float whole){
        return part / whole * 100f;
    }

    public void SetStatisticalValues (List<GameStatistics> gameStatistics){
        _combinedStatistics = new GameStatistics();
        
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
            StatCategories.Average => CalculateAverage(),
            _ => -1f,
        };
    }

    private float CalculateAverage (){
        float average = (float)Math.Round(_combinedStatistics.Score / _combinedStatistics.Games, 2);
        LoggingManager.Instance.LogInformation($"The Average was calculated as being {average}, for the {_combinedStatistics.Games} games bowled. Based off the combined score of {_combinedStatistics.Score}");

        return average;
    }
}

/*
    205
    4 strikes
    6 spares
    0 open

    192
    4 strikes
    5 spares
    1 open

    234
    8 strikes
    3 spares
    0 open
*/