namespace BowlingCoacher.Backend;

internal class Frame {
    public int FirstShotValue { get; set; }
    public int SecondShotValue { get; set; }
    public bool IsStrike => FirstShotValue == 10;
    public bool IsSpare => (FirstShotValue + SecondShotValue == 10);
    public bool IsOpenFrame => (FirstShotValue + SecondShotValue < 10);
    public bool IsMissedSpare => (FirstShotValue < 10 && FirstShotValue + SecondShotValue == 10 && SecondShotValue < 10);
    public int Score => FirstShotValue + SecondShotValue;
}