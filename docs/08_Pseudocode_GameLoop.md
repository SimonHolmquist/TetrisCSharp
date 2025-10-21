# 08 — Pseudocode Game Loop (v2)

El loop distingue **actualización** (tick de física/entrada) y **render**, para reducir *flicker* y facilitar tests.

```csharp
Bootstrap(); // DI de IClock, IRenderer, IInputProvider, IRandomizer, IScoreStore, IScoring

var state = GameFactory.CreateNew(); // crea Board, CurrentPiece, NextPiece, Config
ShowMainMenu();

while (!exit)
{
    switch (ui.Mode)
    {
        case UIMode.MainMenu:
            ui.DrawMainMenu();
            var cmd = input.ReadMenuCommand();
            if (cmd == MenuCmd.Start) state = GameFactory.CreateNew();
            if (cmd == MenuCmd.Start) ui.Mode = UIMode.Playing;
            if (cmd == MenuCmd.Ranking) ui.Mode = UIMode.Ranking;
            if (cmd == MenuCmd.Exit) exit = true;
            break;

        case UIMode.Playing:
            var now = clock.Now;
            HandleUserInput(state); // mueve/rota/drop con colisiones + scoring por drop

            if (now - lastGravityTick >= GravityMs(state.Level))
            {
                if (!TryMoveDown(state))
                {
                    LockCurrentPiece(state);
                    var lines = board.ClearLines();
                    scoring.AddLineClear(lines, state.Level);
                    state.LinesCleared += lines;
                    if (state.LinesCleared >= LinesForNextLevel(state.Level))
                        state.Level++;

                    if (!SpawnNextPiece(state))
                        ui.Mode = UIMode.GameOver;
                }
                lastGravityTick = now;
            }

            renderer.Render(state); // diff rendering para minimizar flicker
            break;

        case UIMode.Paused:
            ui.DrawPause();
            ui.Mode = HandlePauseMenu(input);
            break;

        case UIMode.Ranking:
            ui.DrawRanking(scoreStore.GetTop(10));
            ui.Mode = HandleBackToMenu(input);
            break;

        case UIMode.GameOver:
            ui.DrawGameOver(state.Score);
            var alias = ui.PromptAlias();
            scoreStore.TryAdd(new ScoreEntry(alias, state.Score, clock.UtcNow));
            ui.Mode = UIMode.MainMenu;
            break;
    }
}
```

## Rotación con SRS‑lite (wall‑kicks mínimos)
```csharp
bool TryRotate(State s, RotationDir dir)
{
    var target = s.CurrentPiece.Rotate(dir);
    foreach (var off in RotationSystem.GetOffsets(target.Type, s.CurrentPiece.Rotation, target.Rotation))
    {
        var moved = target.WithOrigin(s.CurrentPiece.Origin.Translate(off.X, off.Y));
        if (board.IsValidPosition(moved)) { s.CurrentPiece = moved; return true; }
    }
    return false;
}
```

## Hard/Soft Drop
```csharp
int SoftDrop(State s)
{
    int cells = 0;
    while (TryMoveDown(s)) { cells++; }
    scoring.AddSoftDrop(cells);
    return cells;
}
int HardDrop(State s)
{
    int cells = 0;
    while (TryMoveDown(s)) { cells++; }
    scoring.AddHardDrop(cells);
    // al finalizar, se bloqueará en el próximo tick por imposibilidad de bajar.
    return cells;
}
```

## Render incremental (idea)
- Mantener un *frame buffer* previo (char[,]).
- Dibujar solo diferencias (tablero, pieza activa, HUD).
- Cadencia objetivo: 30–60 FPS (independiente del *gravity tick*).