using FluentAssertions;
using TetrisCSharp.Infrastructure.Scoring;

namespace TetrisCSharp.Tests
{
    public class ScoringFormulaTests
    {
        [Theory]
        [InlineData(1, 0, 40)]
        [InlineData(2, 0, 100)]
        [InlineData(3, 0, 300)]
        [InlineData(4, 0, 1200)]
        [InlineData(4, 3, 4800)] // ejemplo del doc (nivel 3) :contentReference[oaicite:13]{index=13}
        public void Line_Clear_Score_Matches_Spec(int lines, int level, int expected)
        {
            ScoringService s = new(); // o IScoring concreto según tu implementación
            s.AddLineClear(lines, level); // 40/100/300/1200 × (level+1) :contentReference[oaicite:14]{index=14}
            s.Total.Should().Be(expected);
        }

        [Fact]
        public void Drop_Scoring_Matches_Spec()
        {
            ScoringService s = new();
            s.AddSoftDrop(7);  // +1 por celda :contentReference[oaicite:15]{index=15}
            s.AddHardDrop(7);  // +2 por celda
            s.Total.Should().Be(7 * 1 + 7 * 2);
        }
    }
}
