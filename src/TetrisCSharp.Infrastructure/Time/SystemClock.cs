using TetrisCSharp.Application.Abstractions;

namespace TetrisCSharp.Infrastructure.Time;
public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
    public long Millis => Environment.TickCount64;
}
