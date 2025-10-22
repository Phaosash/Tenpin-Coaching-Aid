using BowlingCoacher.Backend.Classes;
using BowlingCoacher.Shared.DataObjects;
using ErrorLogging;

namespace BowlingCoacher.Shared.Classes;

internal class HomePageManager {
    public StatisticsObject Statistics { get; set; } = new StatisticsObject();
    public StatisticsObject RecentStatistics { get; set; } = new StatisticsObject();

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

            applicationManager.AddNewStatisticalData(passObject);
        
            DisplayAllTimeAverage();
            DisplayOverallPercentages();
            DisplayResentAverage();
            DisplayResentPercentages();
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Encountered an unexpected problem while attempting to submit the score form.");
        }
    }

    //  This method is used to update the DTO component that is used for displaying the average to the user.
    private void DisplayAllTimeAverage (){
        try {
            float average = applicationManager.GetOverallAverage();

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
    private void DisplayOverallPercentages (){        
        try {
            Statistics.StrikePercentage = applicationManager.GetOverallStrikePercentage().ToString("n2") + "%";
            Statistics.SparePercentage = applicationManager.GetOverallSparePercentage().ToString("n2") + "%";
            Statistics.OpenPercentage = applicationManager.GetOverallOpenPercentage().ToString("n2") + "%";
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to update the percentage details for display.");
        }
    }

    private void DisplayResentAverage (){
        try {
            float average = applicationManager.GetRecentAverage();

            if (average == -1.0f){
                Statistics.UserFeedback = "Warning: Failed to get the average. Something went wrong.";
                average = 0.0f;
            }

            RecentStatistics.Average = average;
        } catch (Exception ex){
            LoggingManager.Instance.LogError(ex, "Failed to display the most recent average, something went wrong.");
        }
    }

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