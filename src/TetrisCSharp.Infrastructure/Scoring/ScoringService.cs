
using TetrisCSharp.Application.Abstractions;

namespace TetrisCSharp.Infrastructure.Scoring;
public sealed class ScoringService : IScoring
{
    private int _committed;
    private int _pending; // soft/hard drops antes del lock

    public int Total => checked(_committed + _pending);

    public int PendingDropPoints => throw new NotImplementedException();

    public void AddLineClear(int lines, int level)
    {
        int baseScore = lines switch
        {
            1 => 40,
            2 => 100,
            3 => 300,
            4 => 1200,
            _ => 0
        };
        _committed = checked(_committed + baseScore * (level + 1));
    }

    public void AddSoftDrop(int cells)
    {
        if (cells > 0)
        {
            _pending = checked(_pending + cells * 1);
        }
    }

    public void AddHardDrop(int cells)
    {
        if (cells > 0)
        {
            _pending = checked(_pending + cells * 2);
        }
    }

    public int CommitPending()
    {
        int added = _pending;
        if (added != 0)
        {
            _committed = checked(_committed + added);
            _pending = 0;
        }
        return added;
    }

    public void Reset()
    {
        _committed = 0;
        _pending = 0;
    }
}

