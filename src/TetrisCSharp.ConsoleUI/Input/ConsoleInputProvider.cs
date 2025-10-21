using TetrisCSharp.Application.Abstractions;

namespace TetrisCSharp.ConsoleUI.Input;

public sealed class ConsoleInputProvider : IInputProvider
{
    public IEnumerable<InputEvent> Poll()
    {
        var list = new List<InputEvent>();
        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);
            var flags = Map(key);
            if (flags != KeyFlags.None)
                list.Add(new InputEvent(flags, KeyEventType.Down, Environment.TickCount64));
        }
        return list;
    }

    private static KeyFlags Map(ConsoleKeyInfo k)
    {
        return k.Key switch
        {
            ConsoleKey.LeftArrow or ConsoleKey.D7 => KeyFlags.Left,
            ConsoleKey.RightArrow or ConsoleKey.D9 => KeyFlags.Right,
            ConsoleKey.DownArrow or ConsoleKey.D4 => KeyFlags.Soft,
            ConsoleKey.UpArrow or ConsoleKey.D8 when (k.Modifiers & ConsoleModifiers.Control) != 0 => KeyFlags.RotateCCW,
            ConsoleKey.UpArrow or ConsoleKey.D8 => KeyFlags.RotateCW,
            ConsoleKey.D5 or ConsoleKey.Spacebar when (k.Modifiers & ConsoleModifiers.Shift) == 0 => KeyFlags.Hard,
            ConsoleKey.H => KeyFlags.Help,
            ConsoleKey.P or ConsoleKey.Escape => KeyFlags.Pause,
            ConsoleKey.Enter => KeyFlags.Confirm,
            ConsoleKey.Spacebar => KeyFlags.Restart,
            _ => KeyFlags.None
        };
    }
}
