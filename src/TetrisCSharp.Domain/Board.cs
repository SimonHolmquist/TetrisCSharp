namespace TetrisCSharp.Domain;

public sealed class Board
{
    public const int VisibleHeight = 20;
    public const int HiddenRows = 2;
    public const int Width = 10;
    public const int TotalHeight = VisibleHeight + HiddenRows;

    private readonly CellState[,] _cells = new CellState[Width, TotalHeight];

    public bool IsInside(Coord c) => c.X >= 0 && c.X < Width && c.Y >= 0 && c.Y < TotalHeight;

    public bool IsEmpty(Coord c) => IsInside(c) && _cells[c.X, c.Y] == CellState.Empty;

    public bool IsValidPosition(Piece piece)
    {
        // Scaffold: validar que todos los bloques estén dentro y vacíos (implementación mínima para smoke)
        foreach (var b in piece.Blocks)
        {
            var abs = piece.Origin.Translate(b.X, b.Y);
            if (!IsInside(abs) || !IsEmpty(abs)) return false;
        }
        return true;
    }

    public bool TryMove(Piece piece, int dx, int dy)
    {
        var moved = piece.WithOrigin(piece.Origin.Translate(dx, dy));
        return IsValidPosition(moved);
    }

    public bool TryRotate(Piece piece, Rotation targetRotation)
    {
        // Stub: sin transformación de bloques aún; solo confirma que la posición actual es válida.
        return IsValidPosition(piece);
    }

    public void Lock(Piece piece)
    {
        foreach (var b in piece.Blocks)
        {
            var abs = piece.Origin.Translate(b.X, b.Y);
            if (IsInside(abs)) _cells[abs.X, abs.Y] = CellState.Locked;
        }
    }

    public int ClearLines()
    {
        // Stub: no implementado aún, retorna 0 para scaffold
        return 0;
    }
}
