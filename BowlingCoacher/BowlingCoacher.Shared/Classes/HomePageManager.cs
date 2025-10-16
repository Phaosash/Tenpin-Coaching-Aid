using BowlingCoacher.Backend.Classes;
using BowlingCoacher.Shared.DataObjects;

namespace BowlingCoacher.Shared.Classes;

internal class HomePageManager {
    public StatisticsObject Statistics { get; set; } = new StatisticsObject();

    public string Slogan { get; } = "Your smart coaching companion — designed to track performance, analyze trends, and keep you focused on improvement, one frame at a time."; 

    private readonly ApplicationManager applicationManager = new();

    public async Task InitializeAsync (){
        await Task.Delay(500);
    }

    public void SubmitScoreForm (){
        var passObject = new Tuple<float, float, float, float, float>(
            Statistics.Score,
            Statistics.Games,
            Statistics.Strikes,
            Statistics.Spares,
            Statistics.Opens
        );

        applicationManager.AddStatisticalData(passObject);
        
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