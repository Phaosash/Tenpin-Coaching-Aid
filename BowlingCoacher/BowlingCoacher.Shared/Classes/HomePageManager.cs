using BowlingCoacher.Backend.Classes;
using BowlingCoacher.Shared.DataObjects;
using ErrorLogging;

namespace BowlingCoacher.Shared.Classes;

internal class HomePageManager {
    public StatisticsObject Statistics { get; set; } = new StatisticsObject();

    private readonly ApplicationManager applicationManager = new();

    //  This method is used to submit the details from the score form to the backend for processing.
    //  It does this by creating a Tuple that matches the backends requested Tuple structure, so that
    //  neither the front end or backend have knowledge of each others specific DTO's.
    public void SubmitScoreForm (){
        try {
            var passObject = new Tuple<float, float, float, float, float>(
                Statistics.Score,
                Statistics.Games,
                Statistics.Strikes,
                Statistics.Spares,
                Statistics.Opens
            );

            applicationManager.AddStatisticalData(passObject);
        
            DisplayAverageAsync();
            DisplayPercentages();
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem while attempting to submit the score form.");
        }
    }

    //  This method is used to update the DTO component that is used for displaying the average to the user.
    private void DisplayAverageAsync (){
        try {
            float average = applicationManager.GetAverage();

            if (average == -1.0f){
                Statistics.UserFeedback = "Warning: Failed to get the average. Something went wrong.";
                average = 0.0f;
            }

            Statistics.Average = average;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to display the average something went wrong.");
        }
    }

    //  This method is used to update the DTO components that relate to displaying the percentagges to the user.
    private void DisplayPercentages (){        
        try {
            Statistics.StrikePercentage = applicationManager.GetStrikePercentage().ToString("n2") + "%";
            Statistics.SparePercentage = applicationManager.GetSparePercentage().ToString("n2") + "%";
            Statistics.OpenPercentage = applicationManager.GetOpenPercentage().ToString("n2") + "%";
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to update the percentage details for display.");
        }
    }
}