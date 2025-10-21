using TetrisCSharp.Domain.Geometry;

namespace TetrisCSharp.Domain.Model;

public static class TetrominoDefs
{
    // Coordenadas RELATIVAS al Origin para cada rotación
    // Sistema (0,0) arriba-izquierda, y crece hacia abajo.
    private static readonly IReadOnlyDictionary<(TetrominoType, Rotation), Coord[]> _shapes =
        new Dictionary<(TetrominoType, Rotation), Coord[]>
    {
        // I
        { (TetrominoType.I, Rotation.R0),   new[] { new Coord(-2,0), new(-1,0), new(0,0), new(1,0) } },
        { (TetrominoType.I, Rotation.R90),  new[] { new Coord(0,-1), new(0,0), new(0,1), new(0,2) } },
        { (TetrominoType.I, Rotation.R180), new[] { new Coord(-2,1), new(-1,1), new(0,1), new(1,1) } },
        { (TetrominoType.I, Rotation.R270), new[] { new Coord(-1,-1), new(-1,0), new(-1,1), new(-1,2) } },

        // O (cuadrado no cambia con la rotación)
        { (TetrominoType.O, Rotation.R0),   new[] { new Coord(0,0), new(1,0), new(0,1), new(1,1) } },
        { (TetrominoType.O, Rotation.R90),  new[] { new Coord(0,0), new(1,0), new(0,1), new(1,1) } },
        { (TetrominoType.O, Rotation.R180), new[] { new Coord(0,0), new(1,0), new(0,1), new(1,1) } },
        { (TetrominoType.O, Rotation.R270), new[] { new Coord(0,0), new(1,0), new(0,1), new(1,1) } },

        // T
        { (TetrominoType.T, Rotation.R0),   new[] { new Coord(-1,0), new(0,0), new(1,0), new(0,1) } },
        { (TetrominoType.T, Rotation.R90),  new[] { new Coord(0,-1), new(0,0), new(1,0), new(0,1) } },
        { (TetrominoType.T, Rotation.R180), new[] { new Coord(0,-1), new(-1,0), new(0,0), new(1,0) } },
        { (TetrominoType.T, Rotation.R270), new[] { new Coord(0,-1), new(-1,0), new(0,0), new(0,1) } },

        // S
        { (TetrominoType.S, Rotation.R0),   new[] { new Coord(0,0), new(1,0), new(-1,1), new(0,1) } },
        { (TetrominoType.S, Rotation.R90),  new[] { new Coord(0,-1), new(0,0), new(1,0), new(1,1) } },
        { (TetrominoType.S, Rotation.R180), new[] { new Coord(0,0), new(1,0), new(-1,1), new(0,1) } },
        { (TetrominoType.S, Rotation.R270), new[] { new Coord(0,-1), new(0,0), new(1,0), new(1,1) } },

        // Z
        { (TetrominoType.Z, Rotation.R0),   new[] { new Coord(-1,0), new(0,0), new(0,1), new(1,1) } },
        { (TetrominoType.Z, Rotation.R90),  new[] { new Coord(1,-1), new(0,0), new(1,0), new(0,1) } },
        { (TetrominoType.Z, Rotation.R180), new[] { new Coord(-1,0), new(0,0), new(0,1), new(1,1) } },
        { (TetrominoType.Z, Rotation.R270), new[] { new Coord(1,-1), new(0,0), new(1,0), new(0,1) } },

        // J
        { (TetrominoType.J, Rotation.R0),   new[] { new Coord(-1,0), new(0,0), new(1,0), new(-1,1) } },
        { (TetrominoType.J, Rotation.R90),  new[] { new Coord(0,-1), new(0,0), new(0,1), new(1,1) } },
        { (TetrominoType.J, Rotation.R180), new[] { new Coord(1,-1), new(-1,0), new(0,0), new(1,0) } },
        { (TetrominoType.J, Rotation.R270), new[] { new Coord(-1,-1), new(0,-1), new(0,0), new(0,1) } },

        // L
        { (TetrominoType.L, Rotation.R0),   new[] { new Coord(-1,0), new(0,0), new(1,0), new(1,1) } },
        { (TetrominoType.L, Rotation.R90),  new[] { new Coord(0,-1), new(0,0), new(0,1), new(1,-1) } },
        { (TetrominoType.L, Rotation.R180), new[] { new Coord(-1,-1), new(-1,0), new(0,0), new(1,0) } },
        { (TetrominoType.L, Rotation.R270), new[] { new Coord(-1,1), new(0,-1), new(0,0), new(0,1) } },
    };

    public static Coord[] GetBlocks(TetrominoType type, Rotation rot) => _shapes[(type, rot)];
}
