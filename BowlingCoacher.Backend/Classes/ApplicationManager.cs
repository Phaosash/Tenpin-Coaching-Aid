using BowlingCoacher.Backend.DataModels;
using ErrorLogging;

namespace BowlingCoacher.Backend.Classes;

public class ApplicationManager {
    private readonly DataManager _dataManager;
    private readonly List<GameStatistics> _gameStatistics = [];

    public ApplicationManager (){
        _dataManager = new DataManager();
    }

    //  This method is used to tell the data manager to initialise the statistical data based off the information
    //  supplied throught the Tuple. A Tuple was used as it allows the front end to pass dumb data to the backend
    //  without giving it direct context to the specific DTO's that exist in the backend.
    public void AddStatisticalData (Tuple<float, float, float, float, float> values){
        if (values == null){
            LoggingManager.Instance.LogWarning("Failed to add the statistical data, no data was provided.");
            return;
        }
        
        try {
            GameStatistics game = new(){
                Score = values.Item1,
                Games = values.Item2,
                Strikes = values.Item3,
                Spares = values.Item4,
                Opens = values.Item5
            };

            _gameStatistics.Add(game);
            _dataManager.SetStatisticalValues(_gameStatistics);
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem while attempting initialising the creation of the statistical data.");
        }
    }

    //  This method is used to return the current strike percentage to the front end.
    public float GetStrikePercentage (){
        return GetDataValue(StatCategories.Strikes);
    }

    //  This method is used to return the current spare percentage to the front end.
    public float GetSparePercentage (){
        return GetDataValue(StatCategories.Spares);
    }

    //  This method is used to return the current percentage of open frames to the front end.
    public float GetOpenPercentage (){
        return GetDataValue(StatCategories.OpenFrames);
    }

    //  This method is used to return the current average score to the front end.
    public float GetAverage (){
        return GetDataValue(StatCategories.Average);
    }

    //  This method is used to return the requested numeric value to the calling method. If the returned value is
    //  -1 it indicates that an error was encounterd and the correct value wasn't returned from the data manager.
    //  In which case a value of zero will be returned to the front end to display.
    private float GetDataValue (StatCategories statCategories){
        try {
            float dataValue = _dataManager.GetStatisticsValue(statCategories);

            if (dataValue == -1.0f){
                LoggingManager.Instance.LogWarning("The returned data value indicated that an error was encountered.");
            } else {
                LoggingManager.Instance.LogInformation($"The value of the requested data is: {dataValue}");
            }

            return dataValue;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem, while attempting to get the data value.");
            return -1.0f;
        }
    }
}