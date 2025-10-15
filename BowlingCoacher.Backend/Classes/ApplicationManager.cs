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


}