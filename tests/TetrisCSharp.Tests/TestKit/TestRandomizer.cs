using TetrisCSharp.Application.Abstractions;
using TetrisCSharp.Domain.Model;

namespace TetrisCSharp.Tests.TestKit;

public sealed class TestRandomizer : IRandomizer
{
    private readonly Queue<TetrominoType> _queue = new();

    public TestRandomizer Enqueue(params TetrominoType[] seq)
    {
        foreach (TetrominoType t in seq)
        {
            _queue.Enqueue(t);
        }

        return this;
    }

    public TetrominoType Next()
    {
        throw new NotImplementedException();
    }

    public TetrominoType NextPiece()
    {
        return _queue.Count == 0 ? throw new InvalidOperationException("Randomizer vacío") : _queue.Dequeue();
    }
}

