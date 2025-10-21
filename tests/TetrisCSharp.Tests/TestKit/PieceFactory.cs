using TetrisCSharp.Domain.Geometry;
using TetrisCSharp.Domain.Model;

namespace TetrisCSharp.Tests.TestKit;

public static class PieceFactory
{
    public static Piece I(int x, int y, Rotation r = Rotation.R0)
    {
        return new Piece(TetrominoType.I, r, new Coord(x, y));
    }

    public static Piece O(int x, int y)
    {
        return new Piece(TetrominoType.O, Rotation.R0, new Coord(x, y));
    }

    public static Piece T(int x, int y, Rotation r = Rotation.R0)
    {
        return new Piece(TetrominoType.T, r, new Coord(x, y));
    }

    public static Piece L(int x, int y, Rotation r = Rotation.R0)
    {
        return new Piece(TetrominoType.L, r, new Coord(x, y));
    }

    public static Piece J(int x, int y, Rotation r = Rotation.R0)
    {
        return new Piece(TetrominoType.J, r, new Coord(x, y));
    }

    public static Piece S(int x, int y, Rotation r = Rotation.R0)
    {
        return new Piece(TetrominoType.S, r, new Coord(x, y));
    }

    public static Piece Z(int x, int y, Rotation r = Rotation.R0)
    {
        return new Piece(TetrominoType.Z, r, new Coord(x, y));
    }
}
