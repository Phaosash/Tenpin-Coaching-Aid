namespace BowlingCoacher.Shared.DataObjects;

internal class DisplayObject {
    public string StrikePercentage { get; set; } = string.Empty;
    public string SparePercentage { get; set; } = string.Empty;
    public string OpenPercentage { get; set; } = string.Empty;
    public string UserFeedback { get; set; } = string.Empty;
    public float Average { get; set; }
}