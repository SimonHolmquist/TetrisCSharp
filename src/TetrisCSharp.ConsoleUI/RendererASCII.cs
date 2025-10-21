using TetrisCSharp.Application;
using TetrisCSharp.Domain;

namespace TetrisCSharp.ConsoleUI;

/// <summary>Renderer ASCII monocromo (stub para Fase 0).</summary>
public sealed class RendererASCII : IRenderer
{
    public void Render(GameState state)
    {
        // Fase 0: solo un placeholder mínimo para evitar flicker: no imprime nada aún.
        // (La implementación real se hará en Fase 1 con diff rendering a 30–60 FPS).
    }
}
