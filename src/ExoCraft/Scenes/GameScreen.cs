using ExoCraft.Framework.GameSessions;
using ExoCraft.Framework.GameSystems;
using ExoCraft.Framework.ScreenLayers;
using ExoCraft.Framework.SimWorlds;
using ExoCraft.Framework.VisualWorlds;

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
            DisposeGameSession();
        }
    }

    public override void _ExitTree()
    {
        DisposeGameSession();
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
        _simWorld = new();

        var services = new GameSessionServices
        {
            VisualWorld = GetNode<IVisualWorld>("%VisualWorld"),
            SimWorld = _simWorld,
        };

        var settings = new GameSessionSettings();

        _gameSession = GameSession.CreateInstance();

        _gameSession.ExitRequested += QueueFree;
        _gameSession.Failed += (message) => QueueFree();

        _gameSession.Initialize(services, settings);
        _gameSession.Start();
    }

    private void DisposeGameSession()
    {
        if (_gameSession is null) return;

        _gameSession.Dispose();
        _gameSession = null!;

        _simWorld.Dispose();
        _simWorld = null!;
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
    GameSystemRootNode _gameSystems = null!;

    GameSession _gameSession = null!;
    SimWorld _simWorld = null!;
}
