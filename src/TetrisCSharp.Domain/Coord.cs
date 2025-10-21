namespace TetrisCSharp.Domain;

/// <summary>Coordenada inmutable (0,0) arriba-izquierda.</summary>
public readonly record struct Coord(int X, int Y)
{
    public Coord Translate(int dx, int dy) => new(X + dx, Y + dy);
}
