using BowlingCoacher.Backend.Classes;
using BowlingCoacher.Shared.DataObjects;

namespace BowlingCoacher.Shared.Classes;

internal class HomePageManager {
    public StatisticsObject Statistics { get; set; } = new StatisticsObject();

    public string Slogan { get; } = "Your smart coaching companion — designed to track performance, analyze trends, and keep you focused on improvement, one frame at a time."; 

    private readonly ApplicationManager applicationManager = new();

    public List<ScoreFormObject> ScoreFields { get; set; } = [];

    public async Task InitializeAsync (){
        await Task.Delay(500);
    }

    public void SubmitScoreForm (){
        applicationManager.InitialiseScoreStatistics(Statistics.Score, Statistics.Games);
        applicationManager.SetStrikeValue(Statistics.Strikes);
        applicationManager.SetSpareValue(Statistics.Spares);
        applicationManager.SetOpenValue(Statistics.Opens);
        
        CalculateAverage();
        CalculatePercentages();
    }

    private void CalculateAverage (){
        Statistics.Average = applicationManager.GetAverage();
    }

    private void CalculatePercentages (){
        Statistics.StrikePercentage = applicationManager.GetStrikePercentage().ToString("n2") + "%";
        Statistics.SparePercentage = applicationManager.GetSparePercentage().ToString("n2") + "%";
        Statistics.OpenPercentage = applicationManager.GetOpenPercentage().ToString("n2") + "%";
    }
}