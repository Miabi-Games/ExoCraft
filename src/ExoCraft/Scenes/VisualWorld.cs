using ExoCraft.Framework.Math;
using ExoCraft.Framework.VisualWorlds;

using Godot;

using System;
using System.Text;

namespace ExoCraft.Scenes;

public partial class VisualWorld : Node3D, IVisualWorld
{
    // ─────────────────────────────────────────────────────────────────────────

    [Export(PropertyHint.File, "*.tscn")]
    public string PlayerPawnPath { get; set; } = "";

    [Export(PropertyHint.File, "*.tscn")]
    public string VoxelChunkPath { get; set; } = "";

    // ─────────────────────────────────────────────────────────────────────────

    public override void _Ready()
    {
        _mainCamera = GetNode<Camera3D>("MainCamera");

        ApplyCameraTransform();
        PreloadVisualScenes();
    }

    private void PreloadVisualScenes()
    {
        PreloadScene(nameof(_playerPawnScene), ref _playerPawnScene, PlayerPawnPath);
        PreloadScene(
            nameof(_voxelChunkScene),
            ref _voxelChunkScene,
            VoxelChunkPath);
    }

    public double3xform CameraTransform
    {
        get => _cameraTransform;
        set
        {
            _cameraTransform = value;
            ApplyCameraTransform();
        }
    }

    private void ApplyCameraTransform()
    {
        if (_mainCamera is null) return;

        var rotation = _cameraTransform.rotation;
        var basis = new Basis(
            ToVector3(rotation.x),
            ToVector3(rotation.y),
            ToVector3(rotation.z));

        _mainCamera.Transform = new(basis, Vector3.Zero);
    }

    public void SyncPawn(IVisualPawn visualPawn, double3xform transform)
    {
        transform.position -= _cameraTransform.position;
        visualPawn.SyncPosition(transform);
    }

    public void SyncVoxelChunk(
        IVisualVoxelChunk visualVoxelChunk,
        double3xform transform)
    {
        transform.position -= _cameraTransform.position;
        visualVoxelChunk.SyncPosition(transform);
    }

    // ─────────────────────────────────────────────────────────────────────────

    public IVisualPawn CreatePlayerPawn()
    {
        return InstantiatePawn(nameof(_playerPawnScene), _playerPawnScene);
    }

    public IVisualVoxelChunk CreateVoxelChunk()
    {
        var name = GetDisplayName(nameof(_voxelChunkScene));
        Node node = _voxelChunkScene.Instantiate();

        if (node is not IVisualVoxelChunk voxelChunk)
        {
            string message =
                $"VisualWorld: Scene specified for {name} is not a visual voxel chunk";
            GD.PushError(message);
            node.QueueFree();
            throw new Exception(message);
        }

        AddChild(node);
        return voxelChunk;
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

    private static void PreloadScene(
        string sceneFieldName,
        ref PackedScene scene,
        string path)
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
    private PackedScene _voxelChunkScene = null!;
    private Camera3D _mainCamera = null!;

    private double3xform _cameraTransform = double3xform.identity;

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

    private static Vector3 ToVector3(double3 value)
        => new((float)value.x, (float)value.y, (float)value.z);

    // ─────────────────────────────────────────────────────────────────────────
}
