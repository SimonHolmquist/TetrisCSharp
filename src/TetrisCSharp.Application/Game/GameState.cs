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
    private readonly IScoring _scoring;

    // Timers
    public int GravityMs { get; private set; }
    private long _lastGravityTickMs;
    // DAS/ARR
    private const int DAS = 150, ARR = 50;
    private bool _leftHeld, _rightHeld;
    private long _leftLastRepeat, _rightLastRepeat;

    public GameState(GameConfig cfg, IRandomizer rng, IRotationSystem rot, IScoring scoring)
        : this(new Board(cfg, rot), cfg, rng, scoring, clearBoard: true)
    {
    }

    public GameState(Board board, GameConfig? cfg = null, IRandomizer? rng = null, IScoring? scoring = null)
        : this(board,
              ResolveConfig(board, cfg),
              rng ?? new FallbackRandomizer(),
              scoring ?? new NullScoring(),
              clearBoard: false)
    {
    }

    private GameState(Board board, GameConfig config, IRandomizer rng, IScoring scoring, bool clearBoard)
    {
        Board = board ?? throw new ArgumentNullException(nameof(board));
        Config = config ?? throw new ArgumentNullException(nameof(config));
        _rng = rng ?? throw new ArgumentNullException(nameof(rng));
        _scoring = scoring ?? throw new ArgumentNullException(nameof(scoring));
        Reset(clearBoard);
    }

    public void Start(IClock clock)
    {
        if (!TrySpawnNext())
        {
            return;
        }
        _lastGravityTickMs = clock.Millis;
    }

    public void Reset()
    {
        Reset(true);
    }

    private void Reset(bool clearBoard)
    {
        if (clearBoard)
        {
            Board.ClearAll();
        }
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

    public bool TrySpawnNext()
    {
        _current = new Piece(Next, Rotation.R0, SpawnOrigin());
        Next = _rng.Next();
        if (!Board.IsValidPosition(_current))
        {
            IsOver = true;
            return false;
        }
        return true;
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
    }

    public void ToggleHelp()
    {
        HelpVisible = !HelpVisible;
    }

    private void UpdateGravity(IClock clock)
    {
        long now = clock.Millis;
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
        if (IsPaused || IsOver)
        {
            return;
        }

        if (Board.TryRotate(_current, dir, out Piece? rotated))
        {
            _current = rotated;
        }
    }

    public void HardDrop()
    {
        if (IsPaused || IsOver)
        {
            return;
        }

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

        long now = clock.Millis;
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
        int lines = Board.ClearLines(out _);
        if (lines > 0)
        {
            _scoring.AddLineClear(lines, Level);
        }
        _scoring.CommitPending();
        Score = _scoring.Total;
        LinesCleared += lines;
        UpdateLevelFromLines();
        TrySpawnNext();
    }

    private void UpdateLevelFromLines()
    {
        int newLevel = LinesCleared / Config.LinesPerLevel;
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

    private static GameConfig ResolveConfig(Board board, GameConfig? overrideConfig)
    {
        ArgumentNullException.ThrowIfNull(board);
        GameConfig candidate = overrideConfig ?? board.Config;
        if (candidate.BoardWidth == board.Width && candidate.BoardHeight == board.Height)
        {
            return candidate;
        }

        return new GameConfig
        {
            BoardWidth = board.Width,
            BoardHeight = board.Height,
            LinesPerLevel = candidate.LinesPerLevel,
            GravityBaseMs = candidate.GravityBaseMs,
            GravityPerLevelMs = candidate.GravityPerLevelMs,
            GravityMinMs = candidate.GravityMinMs,
            UseSevenBag = candidate.UseSevenBag
        };
    }

    private sealed class FallbackRandomizer : IRandomizer
    {
        private static readonly TetrominoType[] Sequence = Enum.GetValues<TetrominoType>();
        private int _index;

        public TetrominoType Next()
        {
            TetrominoType value = Sequence[_index % Sequence.Length];
            _index++;
            return value;
        }
    }

    private sealed class NullScoring : IScoring
    {
        public int Total { get; private set; }
        public int PendingDropPoints { get; private set; }

        public void AddLineClear(int lines, int level) { }

        public void AddSoftDrop(int cells) { }

        public void AddHardDrop(int cells) { }

        public int CommitPending() => 0;

        public void Reset()
        {
            Total = 0;
            PendingDropPoints = 0;
        }
    }
}
