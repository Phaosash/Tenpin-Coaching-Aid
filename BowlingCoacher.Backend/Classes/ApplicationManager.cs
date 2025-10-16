using BowlingCoacher.Backend.DataModels;

namespace BowlingCoacher.Backend.Classes;

public class ApplicationManager {
    private readonly DataManager _dataManager;
    private readonly List<GameStatistics> _gameStatistics = [];

    public ApplicationManager (){
        _dataManager = new DataManager();
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
        _dataManager.SetStatisticalValues(_gameStatistics);
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

    private float GetDataValue (StatCategories statCategories){
        return _dataManager.GetStatisticsValue(statCategories);
    }
}