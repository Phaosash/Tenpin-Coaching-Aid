using BowlingCoacher.Backend.DataModels;

namespace BowlingCoacher.Backend.Classes;

internal class DataManager (OutcomeStatistics outcomeStatistics, ScoreStatistics scoreStatistics){
    private OutcomeStatistics _shotStatistics = outcomeStatistics;
    private ScoreStatistics _scoreStatistics = scoreStatistics;

    private static float CalculatePercentage (float part, float whole){
        if (whole <= 0f){
            return -1f;
        }

        return part / whole * 100f;
    }

    public void SetStatisticsValue (float value, StatCategories frameResults){
        switch (frameResults){
            case StatCategories.Strikes:
                _shotStatistics.CategoryA = value;
                break;
            case StatCategories.Spares:
                _shotStatistics.CategoryB = value;
                break;
            case StatCategories.OpenFrames:
                _shotStatistics.CategoryC = value;
                break;
            case StatCategories.TotalShots:
                _shotStatistics.TotalEvents = value;
                break;
        }
    }

    public float GetStatisticsValue (StatCategories statCategories){
        return statCategories switch {
            StatCategories.Strikes => CalculatePercentage(_shotStatistics.CategoryA, _shotStatistics.TotalAttempts),
            StatCategories.Spares => CalculatePercentage(_shotStatistics.CategoryB, _shotStatistics.TotalAttempts),
            StatCategories.OpenFrames => CalculatePercentage(_shotStatistics.CategoryC, _shotStatistics.TotalAttempts),
            StatCategories.Average => _scoreStatistics.AverageScore,
            _ => -1,
        };
    }

    public void SetScoreStatistics (float totalScore, float numberOfGames){
        _scoreStatistics.TotalScore = totalScore;
        _scoreStatistics.NumberOfGames = numberOfGames;
    }
}