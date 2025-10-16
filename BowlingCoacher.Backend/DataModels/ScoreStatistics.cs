namespace BowlingCoacher.Backend.DataModels;

internal struct ScoreStatistics {
    public float TotalScore { get; set; }
    public float NumberOfGames { get; set; }

    public readonly float AverageScore => NumberOfGames == 0 ? 0 : (float)Math.Round(TotalScore / NumberOfGames, 2);
}