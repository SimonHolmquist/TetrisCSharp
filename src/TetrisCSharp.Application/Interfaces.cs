using TetrisCSharp.Domain;

namespace TetrisCSharp.Application;

public interface IRenderer
{
    void Render(GameState state);
}

public interface IInputProvider
{
    // Retorna una secuencia de comandos/inputs normalizados (placeholder para Fase 1)
    IEnumerable<string> Poll();
}

public interface IClock
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}

public interface IRandomizer
{
    TetrominoType NextPieceType();
}

public interface IRotationSystem
{
    IEnumerable<Coord> GetOffsets(TetrominoType type, Rotation from, Rotation to);
}

public interface IScoreStore
{
    IReadOnlyList<ScoreEntry> GetTop(int n);
    bool TryAdd(ScoreEntry entry);
}

public interface IScoring
{
    void AddLineClear(int lines, int level);
    void AddSoftDrop(int cells);
    void AddHardDrop(int cells);
    long Current { get; }
}
