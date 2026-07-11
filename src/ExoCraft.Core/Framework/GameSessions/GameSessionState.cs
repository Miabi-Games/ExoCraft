namespace ExoCraft.Framework.GameSessions;

public enum GameSessionState
{
    Uninitialized,
    Initialized,
    Started,
    Paused, // for future implementation
    Ended,
    Failed,
    Disposed,
}
