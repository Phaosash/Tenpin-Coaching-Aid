using BowlingCoacher.Backend.DataModels;

namespace BowlingCoacher.Backend.Classes;

public class ApplicationManager {
    private DataManager _dataManager;
    private FileManager _fileManager;
    private OutcomeStatistics _outcomeStatistics;
    private ScoreStatistics _scoreStatistics;

    public ApplicationManager (){
        _dataManager = new DataManager(_outcomeStatistics, _scoreStatistics);
        _fileManager = new FileManager();
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