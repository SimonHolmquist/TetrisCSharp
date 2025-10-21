using TetrisCSharp.Domain.Model;
using TetrisCSharp.Domain.Config;

namespace TetrisCSharp.Domain;

public sealed class GameState(GameConfig config)
{
    public Board Board { get; init; } = new(config);
    public Piece? CurrentPiece { get; set; }
    public Piece? NextPiece { get; set; }
    public int Level { get; set; }
    public long Score { get; set; }
    public int LinesCleared { get; set; }
    public bool IsPaused { get; set; }
    public bool IsOver { get; set; }
    public GameConfig Config { get; init; } = config;
}
