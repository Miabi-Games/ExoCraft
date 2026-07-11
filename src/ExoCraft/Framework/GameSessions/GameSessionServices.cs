using ExoCraft.Framework.VisualWorld;

namespace ExoCraft.Framework.GameSessions;

public record class GameSessionServices
{
    public required IVisualWorld VisualWorld { get; init; }
}
