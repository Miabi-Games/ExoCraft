using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;

namespace ExoCraft.Framework.GameSessions;

public interface IGameSession
{
    // ─────────────────────────────────────────────────────────────────────────

    bool HasStarted { get; } // and not ended or failed
    bool IsPaused { get; }

    GameSessionState SessionState { get; }

    void RequestExit();

    // ─────────────────────────────────────────────────────────────────────────

    ISimWorld SimWorld { get; }
    IVisualWorld VisualWorld { get; }

    // ─────────────────────────────────────────────────────────────────────────
}
