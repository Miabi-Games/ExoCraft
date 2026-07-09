using Godot;

namespace ExoCraft.Scenes;

public partial class MainMenu : Control
{
    [Export]
    private bool InGame { get; set; } = false;

    [Export]
    private PackedScene? GameScreen { get; set; }

    [Signal]
    public delegate void ExitMenuEventHandler();

    public override void _Ready()
    {
        GetNodes();
        UpdateButtonVisibility();
        WireSignals();
    }

    private void GetNodes()
    {
        _continueGameButton = GetNode<Button>("%ContinueGameButton");
        _newGameButton = GetNode<Button>("%NewGameButton");
        _returnToGameButton = GetNode<Button>("%ReturnToGameButton");
        _saveButton = GetNode<Button>("%SaveButton");
        _saveAsButton = GetNode<Button>("%SaveAsButton");
        _loadButton = GetNode<Button>("%LoadButton");
        _quickLoadButton = GetNode<Button>("%QuickLoadButton");
        _optionsButton = GetNode<Button>("%OptionsButton");
        _exitToMainMenuButton = GetNode<Button>("%ExitToMainMenuButton");
        _exitToDesktopButton = GetNode<Button>("%ExitToDesktopButton");
    }

    private void UpdateButtonVisibility()
    {
        _continueGameButton.Visible = !InGame && GetContinueIsAvailable();
        _newGameButton.Visible = !InGame;
        _returnToGameButton.Visible = InGame;
        _saveButton.Visible = InGame;
        _saveAsButton.Visible = InGame;
        _loadButton.Visible = true;
        _quickLoadButton.Visible = InGame;
        _optionsButton.Visible = true;
        _exitToMainMenuButton.Visible = InGame;
        _exitToDesktopButton.Visible = true;
    }

    private void WireSignals()
    {
        _newGameButton.Pressed += OnNewGameButtonPressed;
        _returnToGameButton.Pressed += OnReturnToGameButtonPressed;
        _exitToDesktopButton.Pressed += OnExitToDesktopButtonPressed;
    }

    private void OnNewGameButtonPressed()
    {
        if (GameScreen is null)
        {
            GD.PushError("MainMenu: GameScreen is not set.");
            return;
        }

        Error result = GetTree().ChangeSceneToPacked(GameScreen);

        if (result != Error.Ok)
        {
            GD.PushError($"MainMenu: Failed to change scene. Error: {result}");
        }
    }

    private void OnReturnToGameButtonPressed()
    {
        EmitSignal(SignalName.ExitMenu);
    }

    private void OnExitToDesktopButtonPressed()
    {
        GetTree().Quit();
    }

    private static bool GetContinueIsAvailable()
    {
        return false; // for future expansion
    }

    private Button _continueGameButton = null!;
    private Button _newGameButton = null!;
    private Button _returnToGameButton = null!;
    private Button _saveButton = null!;
    private Button _saveAsButton = null!;
    private Button _loadButton = null!;
    private Button _quickLoadButton = null!;
    private Button _optionsButton = null!;
    private Button _exitToMainMenuButton = null!;
    private Button _exitToDesktopButton = null!;
}
