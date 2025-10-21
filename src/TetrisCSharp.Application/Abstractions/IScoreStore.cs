// IScoreStore.cs
namespace TetrisCSharp.Application.Abstractions;
public record ScoreEntry(string Alias, int Points, DateTime TimestampUtc);
public interface IScoreStore
{
    IReadOnlyList<ScoreEntry> GetTop(int n);
    bool TryAdd(ScoreEntry entry);
}
