namespace BowlingCoacher.Backend.DataModels;

internal struct GameStatistics {
    public float Score { get; set; }
    public float Games { get; set; }
    public float Strikes { get; set; }
    public float Spares { get; set; }
    public float Opens { get; set; }

    public readonly float TotalAttempts => Strikes + Spares + Opens;
    public readonly float AverageScore => Games == 0 ? 0 : (float)Math.Round(Score / Games, 2);
}