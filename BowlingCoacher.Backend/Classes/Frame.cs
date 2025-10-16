namespace BowlingCoacher.Backend.Classes;

internal class Frame {
    public int FirstShotValue { get; set; }
    public int SecondShotValue { get; set; }
    public bool IsStrike => FirstShotValue == 10;
    public bool IsSpare => FirstShotValue + SecondShotValue == 10;
    public bool IsOpenFrame => FirstShotValue + SecondShotValue < 10;
    public float Score => FirstShotValue + SecondShotValue;
}