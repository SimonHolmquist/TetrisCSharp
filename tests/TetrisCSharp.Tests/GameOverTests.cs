using FluentAssertions;
using TetrisCSharp.Application.Game;
using TetrisCSharp.Domain.Config;
using TetrisCSharp.Domain.Geometry;
using TetrisCSharp.Domain.Model;

namespace TetrisCSharp.Tests
{
    public class GameOverTests
    {
        private readonly GameConfig _gameConfig = new();

        [Fact]
        public void Invalid_Spawn_Sets_GameOver()
        {
            Board board = new(_gameConfig);
            // Forzá un techo lleno para que el spawn falle
            for (int x = 0; x < 10; x++)
            {
                board.Lock(new Piece(TetrominoType.O, Rotation.R0, new Coord(x, 1)));
            }

            GameState state = new(board);
            bool ok = state.TrySpawnNext(); // el método que uses para spawnear
            ok.Should().BeFalse();
            state.IsOver.Should().BeTrue();
        }
    }
}
