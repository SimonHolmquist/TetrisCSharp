using TetrisCSharp.Domain.Geometry;
using TetrisCSharp.Domain.Model;

namespace TetrisCSharp.Tests.TestKit;


public sealed class BoardBuilder(Board board)
{
    private readonly Board _board = board;

    // string de 10 chars: '.' vacío, 'X' bloque
    public BoardBuilder WithRow(int y, string row)
    {
        if (row.Length != 10)
        {
            throw new ArgumentException("fila debe tener 10 chars");
        }

        for (int x = 0; x < 10; x++)
        {
            if (row[x] == 'X')
            {
                _board.Lock(new Piece(TetrominoType.O, Rotation.R0, new Coord(x, y)));
            }
        }
        return this;
    }

    public Board Build()
    {
        return _board;
    }
}


