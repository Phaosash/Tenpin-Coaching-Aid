using ErrorLogging;

namespace BowlingCoacher.Backend.Classes;

public class StatisticsDisplayHandler {
    private readonly DataManager _dataManager;

    public StatisticsDisplayHandler (){
        _dataManager = new DataManager();
    }

    public async Task InitiaiseDataLoad (){
        await DataManager.LoadDataAsync(_dataManager);
    }

    public float[] GetDataValues (bool useRecent){
        float[] _returnedData = new float[4];

        try {
            _returnedData[0] = _dataManager.CalculateAverage(useRecent);
            _returnedData[1] = _dataManager.CalculateStrikePercentage(useRecent);
            _returnedData[2] = _dataManager.CalculateSparePercentage(useRecent);
            _returnedData[3] = _dataManager.CalculateOpenPercentage(useRecent);

            return _returnedData;
        } catch (IndexOutOfRangeException ex){
            LoggingManager.Instance.LogError(ex, "Attempted to get the data values, but 1 of the index's was out of range.");
            return [];
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to get the data values, encountered an unexpected problem.");
            return [];
        }
    }
}