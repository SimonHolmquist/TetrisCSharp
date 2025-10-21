using System.Text.Json;
using System.Text.RegularExpressions;
using TetrisCSharp.Application.Abstractions;

namespace TetrisCSharp.Infrastructure.Scores;

public sealed class JsonScoreStore : IScoreStore
{
    private readonly string _path;
    private List<ScoreEntry> _scores = new();
    private static readonly Regex AliasRx = new("^[A-Z0-9]{2,10}$", RegexOptions.Compiled);

    public JsonScoreStore(string path)
    {
        _path = path;
        Load();
    }

    public IReadOnlyList<ScoreEntry> GetTop(int n)
        => _scores.OrderByDescending(s => s.Points).ThenBy(s => s.TimestampUtc).Take(n).ToList();

    public bool TryAdd(ScoreEntry entry)
    {
        string alias = entry.Alias.Trim().ToUpperInvariant();
        if (!AliasRx.IsMatch(alias)) return false; // validación :contentReference[oaicite:16]{index=16}
        ScoreEntry e = entry with { Alias = alias };
        _scores.Add(e);
        Save();
        return true;
    }

    private void Load()
    {
        try
        {
            if (!File.Exists(_path)) { _scores = new(); return; }
            string json = File.ReadAllText(_path);
            _scores = JsonSerializer.Deserialize<List<ScoreEntry>>(json) ?? new();
        }
        catch { _scores = new(); } // tolerante a archivo corrupto :contentReference[oaicite:17]{index=17}
    }
    private void Save()
    {
        try
        {
            string? directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string json = JsonSerializer.Serialize(_scores);
            File.WriteAllText(_path, json);
        }
        catch { /* ignore */ }
    }
}
