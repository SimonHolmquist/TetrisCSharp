using TetrisCSharp.Application.Abstractions;
using TetrisCSharp.Application.Game;
using TetrisCSharp.ConsoleUI.Input;
using TetrisCSharp.ConsoleUI.Rendering;
using TetrisCSharp.Domain.Config;
using TetrisCSharp.Infrastructure.Random;
using TetrisCSharp.Infrastructure.Rotation;
using TetrisCSharp.Infrastructure.Scoring;
using TetrisCSharp.Infrastructure.Scores;
using TetrisCSharp.Infrastructure.Time;

GameConfig cfg = new GameConfig();
IClock clock = new SystemClock();
IRandomizer rng = new SevenBagRandomizer();
IRotationSystem rot = new SrsLiteRotationSystem();
IScoring scoring = new ScoringService();
IScoreStore store = new JsonScoreStore(Path.Combine(AppContext.BaseDirectory, "scores.json"));

// Core state (portable)
GameState state = new GameState(cfg, rng, rot, scoring);

// UI dependiente de consola
RendererASCII renderer = new(cfg.BoardWidth + 2 /*marco*/, cfg.BoardHeight - 2 /*visibles*/);
ConsoleInputProvider input = new();

// Orquestador
GameLoopService loop = new(state, clock, renderer, input, store);

// Fixed render cadence ~60 FPS, lógica de gravedad dentro del estado
int targetFrameMs = 16; // ~60Hz
while (loop.Mode != UIMode.Exit)
{
    long start = clock.Millis;
    loop.RunFrame();
    long elapsed = clock.Millis - start;
    long sleep = targetFrameMs - elapsed;
    if (sleep > 0)
    {
        // Evita el error long→int y desborde grande
        Thread.Sleep(TimeSpan.FromMilliseconds(sleep));
    }
}
