using TetrisCSharp.Application.Abstractions;
using TetrisCSharp.Domain.Geometry;
using TetrisCSharp.Domain.Model;

namespace TetrisCSharp.Tests.TestKit;

public sealed class TestRotationSystem : IRotationSystem
    {
        private static readonly Coord[] Offsets =
        [
            new(0,0), new(-1,0), new(1,0), new(0,-1)
        ];

    public IReadOnlyList<Coord> GetOffsets(TetrominoType type, Rotation from, Rotation to)
    {
        return Offsets;
    }
}

