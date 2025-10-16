using BowlingCoacher.Backend.DataModels;

namespace BowlingCoacher.Backend.Classes;

public class ApplicationManager {
    private readonly DataManager _dataManager;
    private OutcomeStatistics _outcomeStatistics;
    private ScoreStatistics _scoreStatistics;
    private readonly List<GameStatistics> _gameStatistics = [];

    public ApplicationManager (){
        _dataManager = new DataManager(_outcomeStatistics, _scoreStatistics);
    }

    public void AddStatisticalData (Tuple<float, float, float, float, float> values){
        GameStatistics game = new(){
            Score = values.Item1,
            Games = values.Item2,
            Strikes = values.Item3,
            Spares = values.Item4,
            Opens = values.Item5
        };

        _gameStatistics.Add(game);
    }

    public float GetStrikePercentage (){
        return GetDataValue(StatCategories.Strikes);
    }

    public float GetSparePercentage (){
        return GetDataValue(StatCategories.Spares);
    }

    public float GetOpenPercentage (){
        return GetDataValue(StatCategories.OpenFrames);
    }

    public float GetAverage (){
        return GetDataValue(StatCategories.Average);
    }

    public void SetStrikeValue (float value){
        InitialiseDataValue(value, StatCategories.Strikes);
    }

    public void SetSpareValue (float value){
        InitialiseDataValue(value, StatCategories.Spares);
    }

    public void SetOpenValue (float value){
        InitialiseDataValue(value, StatCategories.OpenFrames);
    }

    public void SetTotalShotsValue (float value){
        InitialiseDataValue(value, StatCategories.TotalShots);
    }

    private void InitialiseDataValue (float value, StatCategories statCategories){
        _dataManager.SetStatisticsValue(value, statCategories);
    }

    private float GetDataValue (StatCategories statCategories){
        return _dataManager.GetStatisticsValue(statCategories);
    }

    public void InitialiseScoreStatistics (float totalScore, float numberOfGames){
        _dataManager.SetScoreStatistics(totalScore, numberOfGames);
    }
}