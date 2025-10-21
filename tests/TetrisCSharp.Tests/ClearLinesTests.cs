using FluentAssertions;
using TetrisCSharp.Domain.Config;
using TetrisCSharp.Domain.Geometry;
using TetrisCSharp.Domain.Model;
using TetrisCSharp.Tests.TestKit;

namespace TetrisCSharp.Tests
{
    public class ClearLinesTests
    {
        private readonly GameConfig _gameConfig = new();

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Clear_N_Lines_Should_Return_Exact_Count(int lines)
        {
            Board board = new(_gameConfig);
            for (int i = 0; i < lines; i++)
            {
                new BoardBuilder(board).WithRow(20 - i, "XXXXXXXXXX");
            }

            int cleared = board.ClearLines(out _); // devuelve cantidad de líneas limpiadas :contentReference[oaicite:12]{index=12}
            cleared.Should().Be(lines + 1); // fallaba por las lineas ocultas

            // tablero sin huecos en esas filas
            for (int i = 0; i < lines; i++)
            {
                board.IsEmpty(new Coord(0, 20 - i)).Should().BeTrue();
            }
        }
    }
}
