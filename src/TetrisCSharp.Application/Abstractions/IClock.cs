namespace TetrisCSharp.Application.Abstractions;

public interface IClock
{
    DateTime UtcNow { get; }
    long Millis { get; } // práctico para diffs
}
