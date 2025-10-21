using TetrisCSharp.Application.Abstractions;
using TetrisCSharp.Domain.Geometry;
using TetrisCSharp.Domain.Model;

namespace TetrisCSharp.Infrastructure.Rotation;

// Kicks mínimos: (0,0), (-1,0), (1,0), (0,-1)  :contentReference[oaicite:12]{index=12}
public sealed class SrsLiteRotationSystem : IRotationSystem
{
    private static readonly Coord[] Kicks = [new Coord(0, 0), new(-1, 0), new(1, 0), new(0, -1)];
    public IEnumerable<Coord> GetKickOffsets(TetrominoType type, Domain.Model.Rotation from, Domain.Model.Rotation to)
    {
        return Kicks;
    }
}
