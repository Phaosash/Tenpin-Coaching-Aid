using BowlingCoacher.Shared.DataObjects;
using CommunityToolkit.Mvvm.ComponentModel;
using ErrorLogging;

namespace BowlingCoacher.Shared.Classes;

internal partial class GameStateManager: ObservableObject {
    public int GameId { get; private set; }
    [ObservableProperty] private List<GameInputs> _frameValues = [];
    [ObservableProperty] private List<string> _scoreStrings = [];

    public GameStateManager (int gameId){
        GameId = gameId;
        InitialiseLists();
    }

    //  This method is used to initialise both the the lists to contain 10 empty objects,
    //  to allow for manipulation of each element on the front end as required.
    private void InitialiseLists (){
        const int RequiredNumberOfElements = 10;

        try {
            for (int i = 0; i < RequiredNumberOfElements; i++){
                ScoreStrings.Add(string.Empty);
                FrameValues.Add(new GameInputs());
            }

            LoggingManager.Instance.LogInformation($"Successfully initialised both lists. Score list contains {ScoreStrings.Count} empty strings. Frame values contains {FrameValues.Count} new DTO's");
        } catch (IndexOutOfRangeException ex){
            LoggingManager.Instance.LogError(ex, "Failed to initialse the game lists, one or more index's were out of range");
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to initialise the game lists, a non out of range exception has occured.");
        }
    }
}