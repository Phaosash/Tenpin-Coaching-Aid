namespace BowlingCoacher.Shared.DataObjects;

internal class StatisticsObject {
    public float Score { get; set; }
    public float Games { get; set; }
    public float Average { get; set; }
    public float Strikes { get; set; }
    public float Spares { get; set; }
    public float Opens { get; set; }
    public string StrikePercentage { get; set; } = string.Empty;
    public string SparePercentage { get; set; } = string.Empty;
    public string OpenPercentage { get; set; } = string.Empty;
}