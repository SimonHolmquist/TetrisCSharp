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

var cfg = new GameConfig();
IClock clock = new SystemClock();
IRandomizer rng = new SevenBagRandomizer();
IRotationSystem rot = new SrsLiteRotationSystem();
IScoring scoring = new ScoringService();
IScoreStore store = new JsonScoreStore(Path.Combine(AppContext.BaseDirectory, "scores.json"));

// Core state (portable)
var state = new GameState(cfg, rng, rot, scoring);

// UI dependiente de consola
var renderer = new RendererASCII(cfg.BoardWidth + 2 /*marco*/, cfg.BoardHeight - 2 /*visibles*/);
var input = new ConsoleInputProvider();

// Orquestador
var loop = new GameLoopService(state, clock, renderer, input, store);

// Fixed render cadence ~60 FPS, lógica de gravedad dentro del estado
var targetFrameMs = 16; // ~60Hz
while (loop.Mode != UIMode.Exit)
{
    var start = clock.Millis;
    loop.RunFrame();
    var elapsed = clock.Millis - start;
    var sleep = targetFrameMs - (int)elapsed;
    if (sleep > 0) Thread.Sleep(sleep);
}
