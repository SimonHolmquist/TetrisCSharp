using TetrisCSharp.Application.Abstractions;
using TetrisCSharp.Application.Game;
using TetrisCSharp.Domain.Model;
using System.Text;

namespace TetrisCSharp.ConsoleUI.Rendering;

public sealed class RendererASCII : IRenderer
{
    private readonly int _offsetX = 14; // HUD a la izquierda
    private char[,]? _front, _back;
    private readonly int _w, _h;

    public RendererASCII(int width, int height)
    {
        _w = width + _offsetX + 16; // espacio derecha ayuda
        _h = height;
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        _front = new char[_w, _h];
        _back = new char[_w, _h];
    }

    public void Render(GameState s)
    {
        Array.Clear(_back!);
        DrawHUD(s);
        DrawBoard(s);
        DrawHelp(s);

        // diff rendering: escribir solo celdas que cambian
        for (int y = 0; y < _h; y++)
        {
            for (int x = 0; x < _w; x++)
            {
                char ch = _back![x, y];
                if (_front![x, y] != ch)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(ch == '\0' ? ' ' : ch);
                    _front![x, y] = ch;
                }
            }
        }
    }

    private void Put(int x, int y, char c)
    {
        if (x < 0 || y < 0 || x >= _w || y >= _h)
        {
            return;
        }

        _back![x, y] = c;
    }

    private void DrawHUD(GameState s)
    {
        WriteLabel(0, 1, "FILAS");
        WriteLabel(0, 3, s.LinesCleared.ToString());
        WriteLabel(0, 6, "NIVEL");
        WriteLabel(0, 8, s.Level.ToString());
        WriteLabel(0, 11, "SCORE");
        WriteLabel(0, 13, s.Score.ToString());
        WriteLabel(0, 17, "NEXT");
        DrawMiniPiece(0, 19, s.Next);
    }

    private void WriteLabel(int x, int y, string text)
    {
        for (int i = 0; i < text.Length && x + i < _w; i++)
        {
            Put(x + i, y, text[i]);
        }
    }

    private void DrawMiniPiece(int offX, int offY, TetrominoType t)
    {
        Domain.Geometry.Coord[] blocks = TetrominoDefs.GetBlocks(t, Rotation.R0);
        foreach (Domain.Geometry.Coord b in blocks)
        {
            Put(offX + 1 + b.X + 2, offY + 1 + b.Y + 1, '█');
        }
    }

    private void DrawBoard(GameState s)
    {
        // marco
        for (int y = 0; y < s.Board.Height - 2; y++)
        {
            Put(_offsetX - 1, y, '|');
            Put(_offsetX + s.Board.Width, y, '|');
        }

        // celdas fijas (omite 2 filas ocultas)
        for (int y = 2; y < s.Board.Height; y++)
        {
            for (int x = 0; x < s.Board.Width; x++)
            {
                if (s.Board[x, y] == CellState.Locked)
                {
                    Put(_offsetX + x, y - 2, '█');
                }
                else
                {
                    Put(_offsetX + x, y - 2, ' ');
                }
            }
        }

        // pieza actual
        foreach (Domain.Geometry.Coord c in s.Current.Blocks())
        {
            if (c.Y >= 2)
            {
                Put(_offsetX + c.X, c.Y - 2, '█');
            }
        }
    }

    private void DrawHelp(GameState s)
    {
        if (!s.HelpVisible)
        {
            return;
        }

        int x = _offsetX + s.Board.Width + 2;
        string[] lines =
        [
            "AYUDA (H)",
            "←/7: Izquierda",
            "→/9: Derecha",
            "↓/4: Soft drop (+1/celda)",
            "8: Rotar CW, Ctrl+8 CCW",
            "5: Hard drop (+2/celda)",
            "P/Esc: Pausa",
            "Enter: Confirmar",
            "Space: Reiniciar (pausa)"
        ];
        for (int i = 0; i < lines.Length; i++)
        {
            WriteLabel(x, 1 + i, lines[i]);
        }
    }

    public void ShowMainMenu(int selectionIndex, bool blink)
    {
        string title = "TETRIS RETRO";
        int cx = (_w - title.Length) / 2;
        for (int i = 0; i < _w; i++)
        {
            for (int j = 0; j < _h; j++)
            {
                Put(i, j, ' ');
            }
        }

        WriteLabel(cx, 2, title);
        string[] opts = ["START", "RANKING", "EXIT"];
        for (int i = 0; i < opts.Length; i++)
        {
            string txt = (i == selectionIndex && blink) ? $"[{opts[i]}]" : $" {opts[i]} ";
            WriteLabel(cx, 6 + i * 2, txt);
        }
        // pintar frame
        for (int y = 0; y < _h; y++)
        {
            for (int x = 0; x < _w; x++)
            {
                char ch = _back![x, y];
                if (_front![x, y] != ch)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(ch == '\0' ? ' ' : ch);
                    _front![x, y] = ch;
                }
            }
        }
    }

    public void ShowRanking(IReadOnlyList<ScoreEntry> entries)
    {
        for (int i = 0; i < _w; i++)
        {
            for (int j = 0; j < _h; j++)
            {
                Put(i, j, ' ');
            }
        }
        WriteLabel(2, 1, "RANKING TOP-10");
        int row = 3; int rank = 1;
        foreach (ScoreEntry s in entries)
        {
            WriteLabel(2, row, $"{rank,2}. {s.Alias,-10} {s.Points,8}");
            row++;
        }
        Flush();
    }

    public string PromptAlias()
    {
        Console.SetCursorPosition(2, _h - 2);
        Console.Write("Alias (A-Z0-9 2-10): ");
        Console.CursorVisible = true;
        string alias = Console.ReadLine() ?? "";
        Console.CursorVisible = false;
        return alias;
    }

    public void ShowGameOver(int score, bool blink)
    {
        string msg = blink ? "[ GAME OVER ]" : "  GAME OVER  ";
        WriteLabel(2, _h / 2, msg + $"  SCORE: {score}");
        Flush();
    }

    private void Flush()
    {
        for (int y = 0; y < _h; y++)
        {
            for (int x = 0; x < _w; x++)
            {
                char ch = _back![x, y];
                if (_front![x, y] != ch)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(ch == '\0' ? ' ' : ch);
                    _front![x, y] = ch;
                }
            }
        }
    }
}
