namespace TetrisCSharp.Domain;

public sealed class GameConfig
{
    public int GravityMsBase { get; init; } = 800;
    public int GravityPerLevel { get; init; } = 60;
    public int GravityMin { get; init; } = 70;
    public int SoftDropPerCell { get; init; } = 1;
    public int HardDropPerCell { get; init; } = 2;
    public int LinesPerLevel { get; init; } = 10;
    public bool UseSevenBag { get; init; } = true;
    public bool ShowHelp { get; init; } = true;
}

public sealed class GameState
{
    public Board Board { get; init; } = new();
    public Piece? CurrentPiece { get; set; }
    public Piece? NextPiece { get; set; }
    public int Level { get; set; }
    public long Score { get; set; }
    public int LinesCleared { get; set; }
    public bool IsPaused { get; set; }
    public bool IsOver { get; set; }
    public GameConfig Config { get; init; } = new();
}

public sealed record ScoreEntry(string Alias, long Points, DateTime Timestamp);
