// IScoring.cs
namespace TetrisCSharp.Application.Abstractions;
public interface IScoring
{
    int Total { get; }
    int PendingDropPoints { get; }
    void AddLineClear(int lines, int level);
    void AddSoftDrop(int cells);
    void AddHardDrop(int cells);
    int CommitPending();
}
