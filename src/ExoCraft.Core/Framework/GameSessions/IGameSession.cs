using ExoCraft.Framework.VisualWorld;

namespace ExoCraft.Framework.GameSessions;

public interface IGameSession
{
    // ─────────────────────────────────────────────────────────────────────────

    bool HasStarted { get; } // and not ended or failed
    bool IsPaused { get; }

    GameSessionState SessionState { get; }

    void RequestExit();

    // ─────────────────────────────────────────────────────────────────────────

    IVisualWorld VisualWorld { get; }

    // ─────────────────────────────────────────────────────────────────────────
}
