using System.Text.Json;
using TetrisCSharp.Application;
using TetrisCSharp.Domain;

namespace TetrisCSharp.Infrastructure;

public sealed class JsonScoreStore : IScoreStore
{
    private readonly string _filePath;
    private readonly object _gate = new();

    public JsonScoreStore(string filePath)
    {
        _filePath = filePath;
    }

    public IReadOnlyList<ScoreEntry> GetTop(int n)
    {
        var list = Load();
        return list
            .OrderByDescending(s => s.Points)
            .ThenBy(s => s.Timestamp)
            .Take(n)
            .ToList();
    }

    public bool TryAdd(ScoreEntry entry)
    {
        lock (_gate)
        {
            var list = Load().ToList();
            list.Add(entry);
            Save(list);
            return true;
        }
    }

    private IEnumerable<ScoreEntry> Load()
    {
        if (!File.Exists(_filePath)) return Enumerable.Empty<ScoreEntry>();
        try
        {
            var json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<List<ScoreEntry>>(json);
            return data ?? Enumerable.Empty<ScoreEntry>();
        }
        catch
        {
            return Enumerable.Empty<ScoreEntry>();
        }
    }

    private void Save(IEnumerable<ScoreEntry> entries)
    {
        var dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(dir)) Directory.CreateDirectory(dir!);
        var json = JsonSerializer.Serialize(entries);
        File.WriteAllText(_filePath, json);
    }
}
