using FluentAssertions;
using TetrisCSharp.Domain;
using TetrisCSharp.Domain.Config;
using TetrisCSharp.Domain.Model;
using TetrisCSharp.Tests.TestKit;
using Xunit;
using static TetrisCSharp.Tests.TestKit.PieceFactory;

namespace TetrisCSharp.Tests
{
    public class RotationSrsLiteTests
    {
        private readonly GameConfig _gameConfig = new();
        [Fact]
        public void Rotate_Near_Left_Wall_Should_Succeed_With_Kick_Minus1_0()
        {
            var board = new Board(_gameConfig) {};
            var t = T(0, 2, Rotation.R0);

            var ok = board.TryRotate(t, RotationDir.CW, out var rotated);
            ok.Should().BeTrue();
            rotated!.Rotation.Should().Be(Rotation.R90);
            board.IsValidPosition(rotated).Should().BeTrue();
        }

        [Fact]
        public void Rotate_Under_Ceiling_Should_Try_Kick_0_Minus1()
        {
            var board = new Board(rotationSystem: new TestRotationSystem());
            var i = I(5, 1, Rotation.R0); // muy arriba, techo
            var ok = board.TryRotate(i, RotationDir.CW, out var rotated);
            ok.Should().BeTrue();
            rotated!.Rotation.Should().Be(Rotation.R90);
        }
    }
}
