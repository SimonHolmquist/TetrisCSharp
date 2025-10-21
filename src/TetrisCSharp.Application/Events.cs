namespace TetrisCSharp.Application.Events;

public sealed record PieceLocked;
public sealed record LinesCleared(int Count);
public sealed record LevelUp(int NewLevel);
public sealed record GameOver;
public sealed record ScoreChanged(long NewScore);
