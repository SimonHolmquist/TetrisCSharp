using TetrisCSharp.Application;

namespace TetrisCSharp.Infrastructure;

public sealed class SystemClock : IClock
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
