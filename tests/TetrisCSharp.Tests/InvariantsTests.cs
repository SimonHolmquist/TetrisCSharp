using FluentAssertions;
using TetrisCSharp.Domain;
using TetrisCSharp.Domain.Config;
using TetrisCSharp.Domain.Geometry;
using TetrisCSharp.Domain.Model;
using Xunit;

namespace TetrisCSharp.Tests
{
    public class InvariantsTests
    {
    private readonly GameConfig _gameConfig = new();

        [Fact]
        public void No_Cell_Should_Be_Outside_10x22()
        {
            var board = new Board(_gameConfig);
            for (int y = 0; y < 22; y++)
                for (int x = 0; x < 10; x++)
                    board.IsInside(new Coord(x, y)).Should().BeTrue();
        }
    }
}
