namespace TetrisCSharp.Domain.Geometry;

public readonly record struct Coord(int X, int Y)
{
    public Coord Translate(int dx, int dy)
    {
        return new(X + dx, Y + dy);
    }
}
