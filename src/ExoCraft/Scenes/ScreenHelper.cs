using Godot;

namespace ExoCraft.Scenes;

public static class ScreenHelper
{
    private static SceneTree SceneTree => (Engine.GetMainLoop() as SceneTree)!;

    public static readonly (string name, string path) GameMenuOverlay
        = ("game menu overlay", "res://Scenes/GameMenuOverlay.tscn");

    public static readonly (string name, string path) GameScreen
        = ("game screen", "res://Scenes/GameScreen.tscn");

    public static readonly (string name, string path) MainMenuScreen
        = ("main menu screen", "res://Scenes/MainMenuScreen.tscn");

    // —————————————————————————————————————————————————————————————————————————
    // The interface of this class is ugly and inconsistent because it includes
    // the error messaging, but is being left as a future concern.
    // —————————————————————————————————————————————————————————————————————————

    public static Node? LoadOverlay(
        string originator, Node? container, (string name, string path) info)
    {
        return LoadOverlay(originator, container, info, LoadPackedScene(info)!);
    }

    public static Node? LoadOverlay(
        string originator,
        Node? container,
        (string name, string path) info,
        PackedScene preloadedScene)
    {
        var node = preloadedScene?.Instantiate();

        if (node is null)
        {
            (var name, var path) = info;

            GD.PushError(
                $"{originator}: Failed to load {name} at \"{path}\".");
            return null;
        }

        container?.AddChild(node);

        return node;
    }

    public static PackedScene? LoadPackedScene((string name, string path) info)
    {
        return GD.Load<PackedScene>(info.path);
    }

    public static PackedScene? LoadPackedScene(string originator, (string name, string path) info)
    {
        var scene = LoadPackedScene(info);

        if (scene is null)
        {
            (var name, var path) = info;

            GD.PushError(
                $"{originator}: Failed to load {name} at \"{path}\".");
        }

        return scene;
    }

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
