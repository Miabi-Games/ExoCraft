using ExoCraft.Framework.VisualWorld;

using Godot;

using System;
using System.Text;

namespace ExoCraft.Scenes;

public partial class VisualWorld : Node3D, IVisualWorld
{
    // ─────────────────────────────────────────────────────────────────────────

    [Export(PropertyHint.File, "*.tscn")]
    public string PlayerPawnPath { get; set; } = "";

    // ─────────────────────────────────────────────────────────────────────────

    public override void _Ready()
    {
        PreloadPawns();
    }

    private void PreloadPawns()
    {
        PreloadPawn(nameof(_playerPawnScene), ref _playerPawnScene, PlayerPawnPath);
    }

    // ─────────────────────────────────────────────────────────────────────────

    public IVisualPawn CreatePlayerPawn()
    {
        return InstantiatePawn(nameof(_playerPawnScene), _playerPawnScene);
    }

    // ─────────────────────────────────────────────────────────────────────────

    private IVisualPawn InstantiatePawn(string sceneFieldName, PackedScene scene)
    {
        var name = GetDisplayName(sceneFieldName);
        var node = scene.Instantiate();

        if (node is null)
        {
            string message = $"VisualWorld: Unable to instantiate {name}";
            GD.PushError(message);
            throw new Exception(message);
        }

        if (node is not IVisualPawn pawn)
        {
            string message = $"VisualWorld: Scene specified for {name} is not a visual pawn";
            GD.PushError(message);
            throw new Exception(message);
        }

        AddChild(node);
        return pawn;
    }

    private static void PreloadPawn(string sceneFieldName, ref PackedScene scene, string path)
    {
        var name = GetDisplayName(sceneFieldName);

        if (string.IsNullOrEmpty(path))
        {
            GD.PushError(
                $"VisualWorld: Missing resource path for {name}");
            return;
        }

        scene = GD.Load<PackedScene>(path);

        if (scene is null)
        {
            GD.PushError($"VisualWorld: Failed to load scene for {name} from " +
                $"{path}");
        }
    }

    // ─────────────────────────────────────────────────────────────────────────

    // ─────────────────────────────────────────────────────────────────────────
    // Note these fields must follow the naming convention required by
    // GetDisplayName. They must start with an underscore, use camel case, and
    // end with the word "Scene".
    // ─────────────────────────────────────────────────────────────────────────

    private PackedScene _playerPawnScene = null!;

    // ─────────────────────────────────────────────────────────────────────────

    private static string GetDisplayName(string sceneFieldName)
    {
        StringBuilder result = new(sceneFieldName.Length);

        result.Append(char.ToLower(sceneFieldName[1]));

        foreach (char c in sceneFieldName[2..^5])
        {
            if (char.IsAsciiLetterUpper(c))
            {
                result.Append(' ');
                result.Append(char.ToLower(c));
            }
            else
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    // ─────────────────────────────────────────────────────────────────────────
}
