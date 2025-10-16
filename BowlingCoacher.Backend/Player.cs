using BowlingCoacher.Backend.Classes;

namespace BowlingCoacher.Backend;

internal class Player {
    public string Name { get; set; } = string.Empty;
    public List<Game> Games { get; set; } = [];

    public void AddGame (List<Frame> frames){
        Games.Add(new Game { Frames = frames, Date = DateTime.Now });
    }

    public double AverageScore => Games.Average(g => g.TotalScore);
    //public int TotalMissedSpares => Games.Sum(g => g.MissedSpares);

    //public override string ToString (){
    //    return $"Player: {Name}, Average Score: {AverageScore:0.00}, Total Missed Spares: {TotalMissedSpares}";
    //}
}

/*
public static void Main()
    {
        var player = new Player { Name = "John Doe" };

        // Simulating a series of frames for a single game
        var gameFrames = new List<Frame>
        {
            new Frame { FirstRoll = 6, SecondRoll = 4 },  // Spare (not missed)
            new Frame { FirstRoll = 7, SecondRoll = 2 },  // Open frame
            new Frame { FirstRoll = 10, SecondRoll = 0 }, // Strike
            new Frame { FirstRoll = 5, SecondRoll = 5 },  // Spare (not missed)
            new Frame { FirstRoll = 4, SecondRoll = 6 },  // Missed spare (first roll was 4, second roll was 6)
        };

        // Add the game to the player
        player.AddGame(gameFrames);

        // Output the player's performance stats
        Console.WriteLine(player);
    }
*/