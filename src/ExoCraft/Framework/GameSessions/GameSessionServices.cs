using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;

namespace ExoCraft.Framework.GameSessions;

public record class GameSessionServices
{
    public required ISimWorld SimWorld { get; init; }
    public required IVisualWorld VisualWorld { get; init; }
}
