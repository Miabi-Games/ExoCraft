using ExoCraft.Framework;
using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;

using Godot;

namespace ExoCraft.Scenes;

public partial class GameScreen : Control
{
    public override void _Ready()
    {
        ReadyInitializeFields();
        InitializeGameSession();
        InitializeGameSystems();
    }

    public override void _ExitTree()
    {
        if (_gameSession is not null)
        {
            try
            {
                if (_gameSession.HasStarted) _gameSession.End();
            }
            finally
            {
                _gameSession.Dispose();
                _gameSession = null!;
            }
        }

        // This can't be here
        // if (_gameSystems.IsInitialized) _gameSystems.Shutdown();
    }

    public override void _Process(double delta)
    {
        _gameSystems?.Render(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        _gameSystems?.Update(delta);
    }

    public override void _UnhandledInput(InputEvent ev)
    {
        if (!GetShouldAcceptInput()) return;

        if (ev.IsActionPressed("ui_cancel"))
        {
            _gameMenu.Visible = true;
        }

        GetViewport().SetInputAsHandled();
    }

    private static bool GetShouldAcceptInput()
    {
        return InteractiveOverlayTracker.Instance.Count == 0;
    }

    private void InitializeGameSession()
    {
        var services = new GameSessionServices();
        var settings = new GameSessionSettings();

        _gameSession = GameSession.CreateInstance();

        _gameSession.ExitRequested += QueueFree;
        _gameSession.Failed += (message) => QueueFree();

        _gameSession.Initialize(services, settings);
        _gameSession.Start();
    }

    private void InitializeGameSystems()
    {
        _gameSystems.CreateSystem();
        _gameSystems.Initialize();
    }

    private void ReadyInitializeFields()
    {
        _gameMenu = GetNode<GameMenuOverlay>("%GameMenu");
        _gameSystems = GetNode<GameSystemRootNode>("GameSystems");
    }

    GameMenuOverlay _gameMenu = null!;
    GameSession _gameSession = null!;
    GameSystemRootNode _gameSystems = null!;
}
