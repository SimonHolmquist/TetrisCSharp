using TetrisCSharp.Application.Abstractions;
using TetrisCSharp.Domain.Config;
using TetrisCSharp.Domain.Geometry;
using TetrisCSharp.Domain.Model;

namespace TetrisCSharp.Application.Game;

public sealed class GameState
{
    public Board Board { get; }
    private Piece _current = null!;
    public Piece Current => _current;
    public TetrominoType Next { get; private set; }
    public int Level { get; private set; }
    public int Score { get; private set; }
    public int LinesCleared { get; private set; }
    public bool IsPaused { get; private set; }
    public bool IsOver { get; private set; }
    public bool HelpVisible { get; private set; }

    public readonly GameConfig Config;
    private readonly IRandomizer _rng;
    private readonly IRotationSystem _rot;
    private readonly IScoring _scoring;

    // Timers
    public int GravityMs { get; private set; }
    private long _lastGravityTickMs;
    // DAS/ARR
    private const int DAS = 150, ARR = 50;
    private bool _leftHeld, _rightHeld;
    private long _leftLastRepeat, _rightLastRepeat;

    public GameState(GameConfig cfg, IRandomizer rng, IRotationSystem rot, IScoring scoring)
    {
        Config = cfg;
        _rng = rng;
        _rot = rot;
        _scoring = scoring;
        Board = new Board(cfg);
        Reset();
    }

    public void Start(IClock clock)
    {
        SpawnNewCurrent();
        _lastGravityTickMs = clock.Millis;
        if (!Board.IsValidPosition(_current)) IsOver = true;
    }

    public void Reset()
    {
        Board.ClearAll();
        _scoring.Reset();
        LinesCleared = 0;
        Level = 0;
        Score = 0;
        HelpVisible = false;
        IsPaused = false;
        IsOver = false;
        _leftHeld = false;
        _rightHeld = false;
        _leftLastRepeat = 0;
        _rightLastRepeat = 0;
        _lastGravityTickMs = 0;
        UpdateGravityDelay();
        Next = _rng.Next();
        _current = null!;
    }

    private Coord SpawnOrigin()
    {
        // centrado horizontal; Y=0 considerando 2 filas ocultas
        int x = Config.BoardWidth / 2;
        return new Coord(x, 1); // 1 por sobre zona visible
    }

    private void SpawnNewCurrent()
    {
        _current = new Piece(Next, Rotation.R0, SpawnOrigin());
        Next = _rng.Next();
    }

    public void TogglePause() => IsPaused = !IsPaused;
    public void ToggleHelp() => HelpVisible = !HelpVisible;

    private void UpdateGravity(IClock clock)
    {
        var now = clock.Millis;
        if (now - _lastGravityTickMs < GravityMs || IsPaused || IsOver)
        {
            return;
        }

        if (!Board.TryMove(ref _current, 0, 1))
        {
            FinalizePieceLock();
        }
        _lastGravityTickMs = now;
    }

    public void Rotate(RotationDir dir)
    {
        if (IsPaused || IsOver) return;
        Coord[] KicksProvider(Piece r) =>
            _rot.GetKickOffsets(r.Type, _current.Rotation, r.Rotation).ToArray();
        Board.TryRotate(ref _current, dir, p => KicksProvider(p));
    }

    public void HardDrop()
    {
        if (IsPaused || IsOver) return;
        int cells = 0;
        while (Board.TryMove(ref _current, 0, 1)) { cells++; }
        _scoring.AddHardDrop(cells);
        FinalizePieceLock();
    }

    public void SoftDropStep()
    {
        if (IsPaused || IsOver) return;
        if (Board.TryMove(ref _current, 0, 1))
        {
            _scoring.AddSoftDrop(1);
        }
    }

    public void MoveLeftRight(bool left, IClock clock)
    {
        if (IsPaused || IsOver) return;
        ref bool held = ref (left ? ref _leftHeld : ref _rightHeld);
        ref long last = ref (left ? ref _leftLastRepeat : ref _rightLastRepeat);
        int dir = left ? -1 : 1;

        var now = clock.Millis;
        if (!held)
        {
            Board.TryMove(ref _current, dir, 0);
            held = true;
            last = now + DAS;
            return;
        }

        if (now >= last)
        {
            Board.TryMove(ref _current, dir, 0);
            last = now + ARR;
        }
    }
    public void ReleaseLeftRight(bool left)
    {
        if (left)
        {
            _leftHeld = false;
        }
        else
        {
            _rightHeld = false;
        }
    }
    public void Tick(IClock clock)
    {
        UpdateGravity(clock);
    }

    private void FinalizePieceLock()
    {
        Board.Lock(_current);
        var lines = Board.ClearLines(out _);
        if (lines > 0)
        {
            _scoring.AddLineClear(lines, Level);
        }
        _scoring.CommitPending();
        Score = _scoring.Total;
        LinesCleared += lines;
        UpdateLevelFromLines();
        SpawnNewCurrent();
        if (!Board.IsValidPosition(_current))
        {
            IsOver = true;
        }
    }

    private void UpdateLevelFromLines()
    {
        var newLevel = LinesCleared / Config.LinesPerLevel;
        if (newLevel != Level)
        {
            Level = newLevel;
            UpdateGravityDelay();
        }
    }

    private void UpdateGravityDelay()
    {
        GravityMs = Math.Max(Config.GravityMinMs, Config.GravityBaseMs - Level * Config.GravityPerLevelMs);
    }
}
