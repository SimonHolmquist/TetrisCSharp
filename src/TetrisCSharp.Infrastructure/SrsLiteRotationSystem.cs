using TetrisCSharp.Application;
using TetrisCSharp.Domain;

namespace TetrisCSharp.Infrastructure;

public sealed class SrsLiteRotationSystem : IRotationSystem
{
    private static readonly Coord[] Offsets = new[]
    {
        new Coord(0, 0), new Coord(-1, 0), new Coord(1, 0), new Coord(0, -1)
    };

    public IEnumerable<Coord> GetOffsets(TetrominoType type, Rotation from, Rotation to) => Offsets;
}
