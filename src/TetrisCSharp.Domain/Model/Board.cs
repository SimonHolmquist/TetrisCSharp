using TetrisCSharp.Application.Abstractions;
using TetrisCSharp.Domain.Config;
using TetrisCSharp.Domain.Geometry;

namespace TetrisCSharp.Domain.Model;

public sealed class Board
{
    private readonly IRotationSystem _rotationSystem;
    private readonly CellState[,] _cells;
    public int Width { get; }
    public int Height { get; } // incluye 2 filas ocultas arriba
    public GameConfig Config { get; }

    public Board(GameConfig? cfg = null, IRotationSystem? rotationSystem = null)
    {
        Config = cfg ?? new GameConfig();
        Width = Config.BoardWidth;
        Height = Config.BoardHeight;
        _rotationSystem = rotationSystem ?? DefaultRotationSystem.Instance;
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

    public bool TryRotate(Piece piece, RotationDir dir, out Piece? rotated)
    {
        Piece target = piece.Rotate(dir);
        foreach (Coord off in _rotationSystem.GetOffsets(piece.Type, piece.Rotation, target.Rotation))
        {
            Piece candidate = target.WithOrigin(piece.Origin.Translate(off.X, off.Y));
            if (IsValidPosition(candidate))
            {
                rotated = candidate;
                return true;
            }
        }
        rotated = null;
        return false;
    }

    public bool TryRotate(ref Piece piece, RotationDir dir)
    {
        if (TryRotate(piece, dir, out Piece? rotated))
        {
            piece = rotated!;
            return true;
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

    // Hay 2 filas ocultas arriba para spawn/techo; no deben participar del clear
    private const int HiddenTop = 2;

    public int ClearLines(out int[] clearedRows)
    {
        List<int> cleared = new(4);
        for (int y = Height - 1; y >= HiddenTop; y--)
        {
            if (!IsRowFull(y))
            {
                continue;
            }

            cleared.Add(y);

            // Desplazar todo lo que está por encima una fila hacia abajo
            for (int src = y - 1; src >= HiddenTop; src--)
            {
                CopyRowDown(src, src + 1);
            }
            // La fila top visible queda vacía
            ClearRow(HiddenTop);

            // Re-chequear la misma y (cayó una fila nueva aquí)
            y++;
        }

        clearedRows = [.. cleared];
        return cleared.Count;
    }

    private bool IsRowFull(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (_cells[x, y] == CellState.Empty)
            {
                return false;
            }
        }
        return true;
    }

    private void ClearRow(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            _cells[x, y] = CellState.Empty;
        }
    }

    private void CopyRowDown(int src, int dst)
    {
        for (int x = Width - 1; x >= 0; x--)
        {
            _cells[x, dst] = _cells[x, src];
        }
    }

    public CellState this[int x, int y] => _cells[x, y];

    private sealed class DefaultRotationSystem : IRotationSystem
    {
        public static readonly DefaultRotationSystem Instance = new();
        private static readonly Coord[] Kicks = [new(0, 0), new(-1, 0), new(1, 0), new(0, -1)];

        public IEnumerable<Coord> GetKickOffsets(TetrominoType type, Rotation from, Rotation to)
        {
            return Kicks;
        }

        public IReadOnlyList<Coord> GetOffsets(TetrominoType type, Rotation from, Rotation to)
        {
            return Kicks;
        }
    }
}
