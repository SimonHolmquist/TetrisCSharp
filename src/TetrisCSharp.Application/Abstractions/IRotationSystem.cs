// IRotationSystem.cs
using TetrisCSharp.Domain.Geometry;
using TetrisCSharp.Domain.Model;
namespace TetrisCSharp.Application.Abstractions;
public interface IRotationSystem
{
    IEnumerable<Coord> GetKickOffsets(TetrominoType type, Rotation from, Rotation to);
}
