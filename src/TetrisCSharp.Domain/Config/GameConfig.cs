namespace TetrisCSharp.Domain.Config;

public sealed class GameConfig
{
    public int BoardWidth { get; init; } = 10;
    public int BoardHeight { get; init; } = 22; // 20 visibles + 2 ocultas (top)
    public int LinesPerLevel { get; init; } = 10;
    public int GravityBaseMs { get; init; } = 800;
    public int GravityPerLevelMs { get; init; } = 60;
    public int GravityMinMs { get; init; } = 70;
    public bool UseSevenBag { get; init; } = true;
}
