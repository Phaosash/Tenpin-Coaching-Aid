namespace BowlingCoacher.Backend.Classes;

internal class Game {
    public List<Frame> Frames { get; set; } = [];
    public DateTime Date { get; set; }

    public float TotalScore => Frames.Sum(f => f.Score);
    public float MissedSpares => Frames.Count(f => f.IsOpenFrame);
}

/*
public class BowlingGame
{
    public int CalculateScore(List<int> rolls)
    {
        int score = 0;
        int rollIndex = 0;
        
        for (int frame = 0; frame < 10; frame++)
        {
            if (IsStrike(rolls, rollIndex))  // Strike
            {
                score += 10 + StrikeBonus(rolls, rollIndex);
                rollIndex++;
            }
            else if (IsSpare(rolls, rollIndex))  // Spare
            {
                score += 10 + SpareBonus(rolls, rollIndex);
                rollIndex += 2;
            }
            else  // Open Frame
            {
                score += rolls[rollIndex] + rolls[rollIndex + 1];
                rollIndex += 2;
            }
        }

        return score;
    }

    private bool IsStrike(List<int> rolls, int rollIndex)
    {
        return rolls[rollIndex] == 10;
    }

    private bool IsSpare(List<int> rolls, int rollIndex)
    {
        return rolls[rollIndex] + rolls[rollIndex + 1] == 10;
    }

    private int StrikeBonus(List<int> rolls, int rollIndex)
    {
        return rolls[rollIndex + 1] + rolls[rollIndex + 2];
    }

    private int SpareBonus(List<int> rolls, int rollIndex)
    {
        return rolls[rollIndex + 2];
    }
}
*/