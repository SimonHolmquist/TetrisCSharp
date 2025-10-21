// IRenderer.cs
using TetrisCSharp.Application.Game;
namespace TetrisCSharp.Application.Abstractions;
public interface IRenderer
{
    void Render(GameState snapshot);
    void ShowMainMenu(int selectionIndex, bool blink);
    void ShowRanking(IReadOnlyList<ScoreEntry> entries);
    string PromptAlias(); // UI proporcionará una forma básica
    void ShowGameOver(int score, bool blink);
}
