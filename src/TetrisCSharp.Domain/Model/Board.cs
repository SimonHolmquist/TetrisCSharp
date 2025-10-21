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
        if (TryRotate(piece, dir, out var rotated))
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

    private sealed class DefaultRotationSystem : IRotationSystem
    {
        public static readonly DefaultRotationSystem Instance = new();
        private static readonly Coord[] Kicks = [new(0, 0), new(-1, 0), new(1, 0), new(0, -1)];

        public IEnumerable<Coord> GetKickOffsets(TetrominoType type, Rotation from, Rotation to) => Kicks;

        public IReadOnlyList<Coord> GetOffsets(TetrominoType type, Rotation from, Rotation to) => Kicks;
    }
}
