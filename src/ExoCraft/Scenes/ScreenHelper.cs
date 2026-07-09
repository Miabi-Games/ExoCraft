using Godot;

namespace ExoCraft.Scenes;

public static class ScreenHelper
{
    private static SceneTree SceneTree => (Engine.GetMainLoop() as SceneTree)!;

    public static readonly (string name, string path) GameScreen
        = ("game screen", "res://Scenes/GameScreen.tscn");

    public static readonly (string name, string path) MainMenuScreen
        = ("main menu screen", "res://Scenes/MainMenuScreen.tscn");

    public static void SwitchToScreen(string originator, (string name, string path) info)
    {
        (var name, var path) = info;

        Error result = SceneTree.ChangeSceneToFile(path);

        if (result != Error.Ok)
        {
            GD.PushError(
                $"{originator}: Failed to load {name} at \"{path}\". " +
                $"Error: {result}");
        }
    }
}
