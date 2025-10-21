using TetrisCSharp.Application.Abstractions;

namespace TetrisCSharp.Application.Game;

public enum UIMode { MainMenu, Playing, Paused, Ranking, GameOver, Exit }

public sealed class GameLoopService(GameState state, IClock clock, IRenderer renderer, IInputProvider input, IScoreStore scoreStore)
{
    private readonly IClock _clock = clock;
    private readonly IRenderer _renderer = renderer;
    private readonly IInputProvider _input = input;
    private readonly IScoreStore _scores = scoreStore;

    public UIMode Mode { get; private set; } = UIMode.MainMenu;
    private int _menuIndex = 0;
    private bool _blink;
    private long _lastBlinkMs;

    public GameState State { get; } = state;

    public void RunFrame()
    {
        var now = _clock.Millis;
        if (now - _lastBlinkMs > 400) { _blink = !_blink; _lastBlinkMs = now; }

        switch (Mode)
        {
            case UIMode.MainMenu:
                HandleMenuInput();
                break;

            case UIMode.Playing:
                HandleGameInput();
                State.Tick(_clock);
                _renderer.Render(State);
                if (State.IsOver) Mode = UIMode.GameOver;
                break;

            case UIMode.Paused:
                _renderer.Render(State);
                foreach (var e in _input.Poll())
                {
                    if (e.Key.HasFlag(KeyFlags.Restart))
                    { // Space
                        State.Reset();
                        State.Start(_clock);
                        Mode = UIMode.Playing;
                        break;
                    }
                    if (e.Key.HasFlag(KeyFlags.Pause)) { Mode = UIMode.Playing; break; }
                }
                break;

            case UIMode.Ranking:
                _renderer.ShowRanking(_scores.GetTop(10));
                foreach (var e in _input.Poll())
                    if (e.Key.HasFlag(KeyFlags.Back) || e.Key.HasFlag(KeyFlags.Confirm)) { Mode = UIMode.MainMenu; break; }
                break;

            case UIMode.GameOver:
                _renderer.ShowGameOver(State.Score, _blink);
                var alias = _renderer.PromptAlias();
                if (!string.IsNullOrWhiteSpace(alias))
                {
                    _scores.TryAdd(new ScoreEntry(alias, State.Score, _clock.UtcNow));
                }
                Mode = UIMode.MainMenu;
                break;

            case UIMode.Exit:
                break;
        }
    }

    private void HandleMenuInput()
    {
        foreach (var e in _input.Poll())
        {
            if (e.Key.HasFlag(KeyFlags.Soft)) _menuIndex = Math.Min(_menuIndex + 1, 2); // ↓
            if (e.Key.HasFlag(KeyFlags.RotateCW)) _menuIndex = Math.Max(_menuIndex - 1, 0); // ↑
            if (e.Key.HasFlag(KeyFlags.Confirm))
            {
                if (_menuIndex == 0) { State.Reset(); State.Start(_clock); Mode = UIMode.Playing; }
                else if (_menuIndex == 1) Mode = UIMode.Ranking;
                else Mode = UIMode.Exit;
            }
        }
        _renderer.ShowMainMenu(_menuIndex, _blink);
    }

    private void HandleGameInput()
    {
        foreach (var e in _input.Poll())
        {
            if (e.Key.HasFlag(KeyFlags.Help) && e.Type == KeyEventType.Down) State.ToggleHelp();
            if (e.Key.HasFlag(KeyFlags.Pause) && e.Type == KeyEventType.Down) { State.TogglePause(); Mode = State.IsPaused ? UIMode.Paused : UIMode.Playing; }

            if (e.Key.HasFlag(KeyFlags.Left))
            {
                if (e.Type == KeyEventType.Up) State.ReleaseLeftRight(true);
                else State.MoveLeftRight(true, _clock);
            }
            if (e.Key.HasFlag(KeyFlags.Right))
            {
                if (e.Type == KeyEventType.Up) State.ReleaseLeftRight(false);
                else State.MoveLeftRight(false, _clock);
            }
            if (e.Key.HasFlag(KeyFlags.Soft) && e.Type != KeyEventType.Up) State.SoftDropStep();
            if (e.Key.HasFlag(KeyFlags.Hard) && e.Type == KeyEventType.Down) State.HardDrop();
            if (e.Key.HasFlag(KeyFlags.RotateCW) && e.Type == KeyEventType.Down) State.Rotate(Domain.Model.RotationDir.CW);
            if (e.Key.HasFlag(KeyFlags.RotateCCW) && e.Type == KeyEventType.Down) State.Rotate(Domain.Model.RotationDir.CCW);
        }
    }
}
