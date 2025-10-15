namespace BowlingCoacher.Backend.DataModels;

internal struct OutcomeStatistics {
    //  Represents the total number of strikes.
    public float CategoryA { get; set; }
    
    //  Represents the total number of spares.
    public float CategoryB { get; set; }
    
    //  Represents the total number of open frames.
    public float CategoryC { get; set; }
    
    //  Represents the total number of shots rolled.
    public float TotalEvents { get; set; }

    //  Represents the total number of frames bowled.
    public readonly float TotalAttempts => CategoryA + CategoryB + CategoryC;
}