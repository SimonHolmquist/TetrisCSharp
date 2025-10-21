using TetrisCSharp.Application.Abstractions;
using TetrisCSharp.Domain.Model;

namespace TetrisCSharp.Infrastructure.Random;

public sealed class SevenBagRandomizer : IRandomizer
{
    private readonly Queue<TetrominoType> _bag = new();
    private readonly System.Random _rng = new();

    public TetrominoType Next()
    {
        if (_bag.Count == 0) Refill();
        return _bag.Dequeue();
    }

    private void Refill()
    {
        var arr = Enum.GetValues<TetrominoType>().ToArray();
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = _rng.Next(i + 1);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
        foreach (var t in arr) _bag.Enqueue(t);
    }
}
