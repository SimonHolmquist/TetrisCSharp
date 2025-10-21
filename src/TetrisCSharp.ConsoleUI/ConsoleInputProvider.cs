using TetrisCSharp.Application;

namespace TetrisCSharp.ConsoleUI;

/// <summary>Input provider de consola (stub para Fase 0).</summary>
public sealed class ConsoleInputProvider : IInputProvider
{
    public IEnumerable<string> Poll()
    {
        // Fase 0: sin lectura activa; devolver vacío.
        yield break;
    }
}
