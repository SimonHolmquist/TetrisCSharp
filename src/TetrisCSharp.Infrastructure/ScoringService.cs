using TetrisCSharp.Application;

namespace TetrisCSharp.Infrastructure;

public sealed class ScoringService : IScoring
{
    private long _score;

    public long Current => _score;

    public void AddLineClear(int lines, int level)
    {
        int basePoints = lines switch
        {
            1 => 40,
            2 => 100,
            3 => 300,
            4 => 1200,
            _ => 0
        };
        checked { _score += basePoints * (level + 1); }
    }

    public void AddSoftDrop(int cells)
    {
        checked { _score += cells * 1; }
    }

    public void AddHardDrop(int cells)
    {
        checked { _score += cells * 2; }
    }
}
