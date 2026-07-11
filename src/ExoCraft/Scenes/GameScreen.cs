using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.ScreenLayers;
using ExoCraft.Framework.VisualWorld;

using Godot;

namespace ExoCraft.Scenes;

public partial class GameScreen : RootScreenLayer
{
    public override ScreenLayerMouseHandling MouseHandling => ScreenLayerMouseHandling.Captured;

    public override void _Ready()
    {
        ReadyInitializeFields();
    }

    public override void EnterScreenLayer()
    {
        InitializeGameSession();
        InitializeGameSystems();
    }

    public override void ExitScreenLayer()
    {
        try
        {
            if (_gameSystems.IsInitialized) _gameSystems.Shutdown();
            if (_gameSession is not null && _gameSession.HasStarted) _gameSession.End();
        }
        finally
        {
            if (_gameSession is not null)
            {
                _gameSession.Dispose();
                _gameSession = null!;
            }
        }
    }

    public override void _ExitTree()
    {
        // in case ExitScreenLayer wasn't called

        if (_gameSession is not null)
        {
            _gameSession.Dispose();
            _gameSession = null!;
        }
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
            ScreenLayerManager.PushScreenOverlay("GameScreen", _gameMenu);
        }

        GetViewport().SetInputAsHandled();
    }

    private static bool GetShouldAcceptInput()
    {
        return ScreenLayerManager.OverlayCount == 0;
    }

    private void InitializeGameSession()
    {
        var services = new GameSessionServices
        {
            VisualWorld = GetNode<IVisualWorld>("%VisualWorld"),
        };

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
