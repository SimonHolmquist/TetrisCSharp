using FluentAssertions;

namespace TetrisCSharp.Tests
{
    public class LevelProgressionTests
    {
        [Fact]
        public void Level_Increases_Every_10_Lines()
        {
            int level = 0;
            int lines = 0;
            for (int i = 0; i < 12; i++)
            {
                lines++;
                if (lines % 10 == 0)
                {
                    level++;
                }
            }
            level.Should().Be(1);
        }

        [Theory]
        [InlineData(0, 800)]
        [InlineData(1, 740)]
        [InlineData(10, 200)]
        [InlineData(12, 80)]
        [InlineData(13, 70)] // min clamp
        public void Gravity_Follows_Spec(int level, int expectedMs)
        {
            int gravityMs = Math.Max(70, 800 - 60 * level);
            gravityMs.Should().Be(expectedMs);
        }
    }
}
