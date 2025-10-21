using System.Collections.Immutable;
using TetrisCSharp.Domain.Geometry;
using TetrisCSharp.Domain.Model;

namespace TetrisCSharp.Domain;

public sealed class Piece
{
    public TetrominoType Type { get; }
    public Rotation Rotation { get; }
    public Coord Origin { get; }
    public IReadOnlyList<Coord> Blocks { get; }

    public Piece(TetrominoType type, Rotation rotation, Coord origin, IEnumerable<Coord> blocks)
    {
        Type = type;
        Rotation = rotation;
        Origin = origin;
        Blocks = blocks.ToImmutableArray();
    }

    public Piece WithOrigin(Coord newOrigin) => new(Type, Rotation, newOrigin, Blocks);
    public Piece WithRotation(Rotation newRotation, IEnumerable<Coord> newBlocks) => new(Type, newRotation, Origin, newBlocks);
}
