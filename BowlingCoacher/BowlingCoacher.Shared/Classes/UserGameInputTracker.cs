using BowlingCoacher.Shared.DataObjects;
using ErrorLogging;

namespace BowlingCoacher.Shared.Classes;

internal class UserGameInputTracker (ScoreEntryPageManager scoreEntryPageManager){
    private readonly StatisticsObject _gameResults = new();
    private readonly ScoreEntryPageManager _scoreEntryPageManager = scoreEntryPageManager;

    private void SendDataToManager (){
        if (ValidateEntries()){
            _scoreEntryPageManager.AddObjectToList(_gameResults);
        } else {
            LoggingManager.Instance.LogWarning("Failed to send the game data to the manager, the results weren't valid.");
        }
    }

    private void UpdateStrikeCount (){
        _gameResults.Strikes++;
    }

    private void UpdateSpareCount (){
        _gameResults.Spares++;
    }

    private void UpdateOpenFrameCount (){
        _gameResults.Opens++;
    }

    private void UpdateScoreValue (float scoreValue){
        _gameResults.Score = scoreValue;
    }

    private bool ValidateEntries (){
        float targetTotalResults = 10.0f;

        if (CalculateNumberOfEntries() < targetTotalResults){
            return false;
        }

        return true;
    }

    private float CalculateNumberOfEntries (){
        return _gameResults.Strikes + _gameResults.Spares + _gameResults.Opens;
    }
}