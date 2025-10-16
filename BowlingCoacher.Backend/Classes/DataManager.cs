using BowlingCoacher.Backend.DataModels;

namespace BowlingCoacher.Backend.Classes;

internal class DataManager (OutcomeStatistics outcomeStatistics, ScoreStatistics scoreStatistics){
    private OutcomeStatistics _shotStatistics = outcomeStatistics;
    private ScoreStatistics _scoreStatistics = scoreStatistics;
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

    private void SetAnalyticsValues (){
        _shotStatistics.CategoryA = _combinedStatistics.Strikes;
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