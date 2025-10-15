using BowlingCoacher.Backend.Classes;

namespace BowlingCoacher.Shared.Views;

internal class HomePageManager {
    public float Average { get; private set; }
    public string Slogan { get; } = "Your smart coaching companion — designed to track performance, analyze trends, and keep you focused on improvement, one frame at a time."; 

    private readonly float series = 633;
    private readonly float games = 3;
    private readonly ApplicationManager applicationManager = new();

    public async Task InitializeAsync (){
        await Task.Delay(500);
        InitialiseSampleData();
    }

    private void InitialiseSampleData (){
        applicationManager.InitialiseScoreStatistics(series, games);
        CalculateAverage();
    }

    private void CalculateAverage (){
        Average = applicationManager.GetAverage();
    }
}