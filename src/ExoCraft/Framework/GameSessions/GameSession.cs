using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;

using Godot;

using System;

namespace ExoCraft.Framework.GameSessions;

public sealed class GameSession : IGameSession, IDisposable
{
    // ─────────────────────────────────────────────────────────────────────────

    public static IGameSession Instance { get; private set; } = null!;

    public static GameSession CreateInstance()
    {
        if (Instance is not null)
        {
            GD.PushError(
                "GameSession: Can not create more than one global instance " +
                "of GameSession. The existing instance will be replaced.");
        }

        var instance = new GameSession();
        Instance = instance;

        return instance;
    }

    // Note: FailureMessage can be null even when _hasFailed is true (clients
    // aren't required to provide a message).

    // ─────────────────────────────────────────────────────────────────────────

    private bool _hasFailed = false;
    public string? FailureMessage { get; private set; } = null;

    public bool HasStarted =>
        SessionState == GameSessionState.Started ||
        SessionState == GameSessionState.Paused;

    public bool IsPaused => SessionState == GameSessionState.Paused;

    public GameSessionState SessionState { get; private set; }

    public delegate void ExitRequestedEventHandler();
    public event ExitRequestedEventHandler? ExitRequested;

    public delegate void FailedEventHandler(string? message);
    public event FailedEventHandler? Failed;

    // ─────────────────────────────────────────────────────────────────────────

    private GameSession() { }

    public void Dispose()
    {
        if (this == Instance) Instance = null!;
        SessionState = GameSessionState.Disposed;
    }

    public void End()
    {
        if (SessionState == GameSessionState.Ended) return;

        if (!HasStarted) // started or paused
        {
            Fail(
                "GameSession: Session can only be ended when in a started or " +
                "paused state.");
            return;
        }

        SessionState = GameSessionState.Ended;
    }

    public void Fail(string? message = null)
    {
        GD.PushError(message);

        // We can't just use the state because `Disposed` has higher priority
        // than `Failed`.

        if (!_hasFailed)
        {
            GD.PushError("GameSession: *** Failed ***");

            if (SessionState != GameSessionState.Disposed)
            {
                SessionState = GameSessionState.Failed;
            }

            _hasFailed = true;
            FailureMessage = message;

            Failed?.Invoke(message);
        }
    }

    public void Initialize(
        GameSessionServices services, GameSessionSettings settings)
    {
        // Because `Initialize` sets other state information, we don't ignore if
        // it is called multiple times.

        if (SessionState != GameSessionState.Uninitialized)
        {
            Fail(
                "GameSession: Can only initialize an uninitialized game " +
                "session.");
            return;
        }

        SimWorld = services.SimWorld;
        VisualWorld = services.VisualWorld;

        SessionState = GameSessionState.Initialized;
    }

    public void RequestExit()
    {
        ExitRequested?.Invoke();
    }

    public void Start()
    {
        if (SessionState == GameSessionState.Started) return;

        if (SessionState != GameSessionState.Initialized)
        {
            Fail(
                "GameSession: Session can only be started from an " +
                "newly initiliazed state.");
            return;
        }

        SessionState = GameSessionState.Started;
    }

    // ─────────────────────────────────────────────────────────────────────────

    public ISimWorld SimWorld { get; private set; } = null!;
    public IVisualWorld VisualWorld { get; private set; } = null!;

    // ─────────────────────────────────────────────────────────────────────────
}
