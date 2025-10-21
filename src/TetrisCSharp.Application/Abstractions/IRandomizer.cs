// IRandomizer.cs
using TetrisCSharp.Domain.Model;
namespace TetrisCSharp.Application.Abstractions;
public interface IRandomizer { TetrominoType Next(); }
