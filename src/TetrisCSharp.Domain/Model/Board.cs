using TetrisCSharp.Domain.Config;
using TetrisCSharp.Domain.Geometry;

namespace TetrisCSharp.Domain.Model;

public sealed class Board
{
    private readonly CellState[,] _cells;
    public int Width { get; }
    public int Height { get; } // incluye 2 filas ocultas arriba

    public Board(GameConfig cfg)
    {
        Width = cfg.BoardWidth;
        Height = cfg.BoardHeight;
        _cells = new CellState[Width, Height];
        ClearAll();
    }

    public void ClearAll()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _cells[x, y] = CellState.Empty;
            }
        }
    }

    public bool IsInside(Coord c)
    {
        return c.X >= 0 && c.X < Width && c.Y >= 0 && c.Y < Height;
    }

    public bool IsEmpty(Coord c)
    {
        return IsInside(c) && _cells[c.X, c.Y] == CellState.Empty;
    }

    public bool IsValidPosition(Piece p)
    {
        return p.Blocks().All(IsEmpty);
    }

    public bool TryMove(ref Piece p, int dx, int dy)
    {
        Piece moved = p.WithOrigin(p.Origin.Translate(dx, dy));
        if (!IsValidPosition(moved))
        {
            return false;
        }

        p = moved;
        return true;
    }

    public bool TryRotate(ref Piece p, RotationDir dir, Func<Piece, IEnumerable<Coord>> kickOffsets)
    {
        Piece rotated = p.Rotate(dir);
        foreach (Coord off in kickOffsets(rotated))
        {
            Piece candidate = rotated.WithOrigin(p.Origin.Translate(off.X, off.Y));
            if (IsValidPosition(candidate)) { p = candidate; return true; }
        }
        return false;
    }

    public void Lock(Piece p)
    {
        foreach (Coord c in p.Blocks())
        {
            if (IsInside(c))
            {
                _cells[c.X, c.Y] = CellState.Locked;
            }
        }
    }

    public int ClearLines(out List<int> clearedRows)
    {
        clearedRows = [];
        for (int y = 0; y < Height; y++)
        {
            bool full = true;
            for (int x = 0; x < Width; x++)
            {
                if (_cells[x, y] == CellState.Empty) { full = false; break; }
            }

            if (full)
            {
                clearedRows.Add(y);
            }
        }

        if (clearedRows.Count == 0)
        {
            return 0;
        }

        foreach (int row in clearedRows)
        {
            for (int y = row; y > 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    _cells[x, y] = _cells[x, y - 1];
                }
            }

            for (int x = 0; x < Width; x++)
            {
                _cells[x, 0] = CellState.Empty;
            }
        }
        return clearedRows.Count;
    }

    public CellState this[int x, int y] => _cells[x, y];
}
