using System.Collections.Generic;
using TetrisCSharp.Application;
using TetrisCSharp.Domain;

namespace TetrisCSharp.Infrastructure;

public sealed class SevenBagRandomizer : IRandomizer
{
    private readonly Queue<TetrominoType> _bag = new();

    private static readonly TetrominoType[] All = new[]
    {
        TetrominoType.I, TetrominoType.O, TetrominoType.T, TetrominoType.S,
        TetrominoType.Z, TetrominoType.J, TetrominoType.L
    };

    private readonly Random _rng = new();

    public TetrominoType NextPieceType()
    {
        if (_bag.Count == 0)
        {
            var list = new List<TetrominoType>(All);
            for (int i = 0; i < list.Count; i++)
            {
                int j = _rng.Next(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
            foreach (var t in list) _bag.Enqueue(t);
        }
        return _bag.Dequeue();
    }
}
