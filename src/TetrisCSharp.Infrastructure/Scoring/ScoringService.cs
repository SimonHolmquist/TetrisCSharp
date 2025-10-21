using TetrisCSharp.Application.Abstractions;

namespace TetrisCSharp.Infrastructure.Scoring;

public sealed class ScoringService : IScoring
{
    public int Total { get; private set; }
    public int PendingDropPoints { get; private set; }

    public void AddLineClear(int lines, int level)
    {
        if (lines <= 0) return;
        int @base = lines switch { 1 => 40, 2 => 100, 3 => 300, 4 => 1200, _ => 0 };
        Total += @base * (level + 1); // :contentReference[oaicite:13]{index=13}
    }
    public void AddSoftDrop(int cells) { if (cells > 0) PendingDropPoints += cells * 1; } // :contentReference[oaicite:14]{index=14}
    public void AddHardDrop(int cells) { if (cells > 0) PendingDropPoints += cells * 2; } // :contentReference[oaicite:15]{index=15}
    public int CommitPending() { int add = PendingDropPoints; PendingDropPoints = 0; Total += add; return add; }
}
