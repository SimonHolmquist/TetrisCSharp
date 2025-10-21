using FluentAssertions;
using TetrisCSharp.Domain.Config;
using TetrisCSharp.Domain.Model;
using TetrisCSharp.Tests.TestKit;
using static TetrisCSharp.Tests.TestKit.PieceFactory;

namespace TetrisCSharp.Tests
{
    public class BoardCollisionTests
    {
        private readonly GameConfig _gameConfig = new(); // restablecer configuración global si es necesario

        [Fact]
        public void Move_Left_Against_Wall_Should_Fail_And_Not_Mutate()
        {
            Board board = new(_gameConfig); // Width=10, Height=20 + 2 ocultas :contentReference[oaicite:10]{index=10}
            Piece current = O(0, 2);
            bool ok = board.TryMove(ref current, dx: -1, dy: 0); // contrato TryMove(out) según tu Fase 1

            ok.Should().BeFalse();
            board.IsValidPosition(current).Should().BeTrue(); // estado intacto
        }

        [Fact]
        public void Move_Down_Onto_Stack_Should_Fail()
        {
            Board board = new(_gameConfig);
            new BoardBuilder(board).WithRow(3, "XXXXXXXXXX");
            Piece p = O(4, 2);
            bool ok = board.TryMove(ref p, 0, +1);
            ok.Should().BeFalse();
        }

        [Fact]
        public void Move_Right_Against_Wall_Should_Fail()
        {
            Board board = new(_gameConfig);
            Piece p = O(9, 2);
            bool ok = board.TryMove(ref p, +1, 0);
            ok.Should().BeFalse();
        }
    }
}
