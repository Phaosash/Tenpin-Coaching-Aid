using BowlingCoacher.Backend.Classes;
using BowlingCoacher.Shared.DataObjects;
using ErrorLogging;

namespace BowlingCoacher.Shared.Classes;

internal class HomePageManager {
    public StatisticsObject Statistics { get; set; } = new();
    public DisplayObject RecentStatistics { get; set; } = new();
    public DisplayObject CombinedStatistics { get; set; } = new();

    private readonly ApplicationManager applicationManager;

    public HomePageManager (){
        applicationManager = new ApplicationManager();
    }

    //  This method is used to initialise the loading of the application on the backend.
    public async Task InitialiseDataLoadAsync (){
        await ApplicationManager.CreateAsync(applicationManager);
        DisplayData();
    }

    //  This method is used to submit the details from the score form to the backend for processing.
    //  It does this by creating a Tuple that matches the backends requested Tuple structure, so that
    //  neither the front end or backend have knowledge of each others specific DTO's.
    public async Task SubmitScoreFormAsync (){
        try {
            var passObject = new Tuple<float, float, float, float, float>(
                Statistics.Score,
                Statistics.Games,
                Statistics.Strikes,
                Statistics.Spares,
                Statistics.Opens
            );

            await applicationManager.AddNewStatisticalDataAsync(passObject);

            DisplayData();
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem while attempting to submit the score form.");
        }
    }

    //  This method is used to simply call the methods to update the different parts of the UI.
    private void DisplayData (){
        DisplayAllTimeAverage();
        DisplayOverallPercentages();
        DisplayResentAverage();
        DisplayResentPercentages();
    }

    //  This method is used to update the DTO component that is used for displaying the average to the user.
    private void DisplayAllTimeAverage (){
        try {
            float average = applicationManager.GetOverallAverage();

            if (average == -1.0f){
                CombinedStatistics.UserFeedback = "Warning: Failed to get the average. Something went wrong.";
                average = 0.0f;
            }

            CombinedStatistics.Average = average;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to display the average something went wrong.");
        }
    }

    //  This method is used to update the DTO components that relate to displaying the percentagges to the user.
    private void DisplayOverallPercentages (){        
        try {
            CombinedStatistics.StrikePercentage = applicationManager.GetOverallStrikePercentage().ToString("n2") + "%";
            CombinedStatistics.SparePercentage = applicationManager.GetOverallSparePercentage().ToString("n2") + "%";
            CombinedStatistics.OpenPercentage = applicationManager.GetOverallOpenPercentage().ToString("n2") + "%";
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to update the percentage details for display.");
        }
    }

    //  This method is used to get the average from the backend, and return it to the front end for display.
    //  If something goes wrong while calculating the average, it should display a value of zero to the user.
    private void DisplayResentAverage (){
        try {
            float average = applicationManager.GetRecentAverage();

            if (average == -1.0f){
                RecentStatistics.UserFeedback = "Warning: Failed to get the average. Something went wrong.";
                average = 0.0f;
            }

            RecentStatistics.Average = average;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to display the most recent average, something went wrong.");
        }
    }

    //  This method is used to display the most recent data to the user by pulling the relevent data from the backend.
    //  It achives this by setting the values of the DTO to the data from the backend.
    private void DisplayResentPercentages (){
        try {
            RecentStatistics.StrikePercentage = applicationManager.GetRecentStrikePercentage().ToString("n2") + "%";
            RecentStatistics.SparePercentage = applicationManager.GetRecentSparePercentage().ToString("n2") + "%";
            RecentStatistics.OpenPercentage = applicationManager.GetRecentOpenPercentage().ToString("n2") + "%";
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to update the recent percentages for display.");
        }
    }
}