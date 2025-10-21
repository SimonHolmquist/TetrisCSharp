// IInputProvider.cs
namespace TetrisCSharp.Application.Abstractions;

[Flags]
public enum KeyFlags { None = 0, Left = 1, Right = 2, Soft = 4, RotateCW = 8, RotateCCW = 16, Hard = 32, Pause = 64, Help = 128, Confirm = 256, Restart = 512, Back = 1024 }

public enum KeyEventType { Down, Held, Up }

public readonly record struct InputEvent(KeyFlags Key, KeyEventType Type, long TimestampMs);

public interface IInputProvider
{
    // no bloqueante; devuelve cero o más eventos desde el último frame
    IEnumerable<InputEvent> Poll();
}
