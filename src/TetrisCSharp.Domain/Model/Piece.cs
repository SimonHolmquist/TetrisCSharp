using TetrisCSharp.Domain.Geometry;

namespace TetrisCSharp.Domain.Model;

public sealed class Piece
{
    public TetrominoType Type { get; }
    public Rotation Rotation { get; }
    public Coord Origin { get; }

    public Piece(TetrominoType type, Rotation rotation, Coord origin)
    {
        Type = type; Rotation = rotation; Origin = origin;
    }

    public IEnumerable<Coord> Blocks()
        => TetrominoDefs.GetBlocks(Type, Rotation).Select(c => Origin.Translate(c.X, c.Y));

    public Piece WithOrigin(Coord o) => new(Type, Rotation, o);

    public Piece Rotate(RotationDir dir)
    {
        Rotation next = dir switch
        {
            RotationDir.CW => Rotation switch
            {
                Rotation.R0 => Rotation.R90,
                Rotation.R90 => Rotation.R180,
                Rotation.R180 => Rotation.R270,
                _ => Rotation.R0
            },
            _ => Rotation switch
            {
                Rotation.R0 => Rotation.R270,
                Rotation.R90 => Rotation.R0,
                Rotation.R180 => Rotation.R90,
                _ => Rotation.R180
            }
        };
        return new Piece(Type, next, Origin);
    }
}
