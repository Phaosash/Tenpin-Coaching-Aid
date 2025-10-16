namespace BowlingCoacher.Backend.DataModels;

internal struct GameStatistics {
    public float Score { get; set; }
    public float Games { get; set; }
    public float Strikes { get; set; }
    public float Spares { get; set; }
    public float Opens { get; set; }

    public readonly float TotalAttempts => Strikes + Spares + Opens;
}

/*
    internal struct GameStatistics {
        public float Score { get; set; }
        public float Games { get; set; }
        public float Strikes { get; set; }
        public float Spares { get; set; }
        public float Opens { get; set; }

        public float Frames => Games * 10;

        public float StrikePercentage => CalculatePercentage(Strikes, Frames);
        public float SparePercentage  => CalculatePercentage(Spares, Frames);
        public float OpenPercentage   => CalculatePercentage(Opens, Frames);

        private static float CalculatePercentage(float part, float whole) =>
            (whole == 0f) ? 0f : part / whole * 100f;
    }
*/