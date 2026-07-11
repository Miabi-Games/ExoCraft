using Godot;

using System.Collections.Generic;

namespace ExoCraft.Framework.ScreenLayers;

public static class ScreenLayerManager
{
    // ─────────────────────────────────────────────────────────────────────────
    // The root screen is loaded from a packed scene resource file using Godot's
    // change scene methods. The screen layer manager provides additional
    // lifecycle callbacks.
    //
    // Overlays are controls in the root scene that can be pushed and popped
    // onto the overlay stack. The overlay stack controls their visiblity and
    // provides additional lifecycle callbacks. Importantly, it does not control
    // their position in the tree, the order in which they are rendered, or the
    // order in which they receive input.
    //
    // The screen layout manager also synchronizes the mouse visibility to match
    // the visibility of screens on the stack.
    // ─────────────────────────────────────────────────────────────────────────

    private static ScreenLayer? _rootScreen = null;
    private static readonly List<ScreenLayer> _overlays = new();

    private static SceneTree SceneTree => (Engine.GetMainLoop() as SceneTree)!;

    public static int OverlayCount => _overlays.Count;
    public static ScreenLayer? TopOverlay => OverlayCount == 0 ? null : _overlays[^1];

    public static void PopRoot(string originator)
    {
        if (_rootScreen is null)
        {
            GD.PushError(
                $"{originator}: Failed to pop the root screen layer. No root " +
                $"screen was popped to pop.");
            return;
        }

        if (OverlayCount > 0) PopScreenOverlay(originator, _overlays[0]);

        _rootScreen.ExitScreenLayer();
        _rootScreen.QueueFree();
        _rootScreen = null;

        SyncMouseState();
    }

    public static void PopScreenOverlay(string originator, ScreenLayer layer)
    {
        if (!_overlays.Contains(layer))
        {
            GD.PushError(
                $"{originator}: Failed to remove screen overlay. It is not " +
                $"being shown.");
            return;
        }

        ScreenLayer? popped;

        do
        {
            var top = TopOverlay!;

            top.ExitScreenLayer();
            top.Visible = false;

            _overlays.RemoveAt(OverlayCount - 1);

            popped = top;
        }
        while (popped != layer);

        SyncMouseState();
    }

    public static void PushScreenOverlay(string originator, ScreenLayer layer)
    {
        if (!layer.IsInsideTree())
        {
            GD.PushError(
                $"{originator}: Can not show the requested screen layer. It " +
                $"is not inside the scene tree.");
            return;
        }

        if (_overlays.Contains(layer))
        {
            GD.PushError(
                $"{originator}: Can not show the requested screen layer. It " +
                $"is already shown.");
            return;
        }

        if (layer.ScreenLayerType != ScreenLayerType.Overlay)
        {
            GD.PushError(
                $"{originator}: Can not show the requested screen layer as " +
                $"an overlay.");
            return;
        }

        _overlays.Add(layer);
        layer.Visible = true;

        layer.EnterScreenLayer();

        SyncMouseState();
    }

    public static void SwitchRootScreenLayer(
        string originator, ScreenLayerInfo layerInfo)
    {
        var layer = LoadScreenLayer(originator, layerInfo);

        if (layer is null) return;

        if (layer.ScreenLayerType != ScreenLayerType.Root)
        {
            GD.PushError(
                $"{originator}: Failed to switch to {layerInfo.Name}. " +
                $"\"{layerInfo.Path}\" is not a root screen layer.");
            return;
        }

        if (_rootScreen is not null) PopRoot(originator);

        Error result = SceneTree.ChangeSceneToNode(layer);

        if (result != Error.Ok)
        {
            GD.PushError(
                $"{originator}: Failed to update scene tree when switching " +
                $"to {layerInfo.Name} at \"{layerInfo.Path}\". Error: " +
                $"{result}.");
            return;
        }

        _rootScreen = layer;
        _rootScreen.Ready += () => _rootScreen.EnterScreenLayer();

        SyncMouseState();
    }

    private static ScreenLayer? LoadScreenLayer(
        string originator, ScreenLayerInfo layerInfo)
    {
        var scene = GD.Load<PackedScene>(layerInfo.Path).Instantiate();

        if (scene is not ScreenLayer layer)
        {
            GD.PushError(
                $"{originator}: Failed to load {layerInfo.Name} at " +
                $"\"{layerInfo.Path}\".");
            return null;
        }

        return layer;
    }

    private static void SyncMouseState()
    {
        if (_rootScreen is null)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            return;
        }

        for (int i = OverlayCount - 1; i >= 0; i--)
        {
            switch (_overlays[i].MouseHandling)
            {
            case ScreenLayerMouseHandling.Captured:
                Input.MouseMode = Input.MouseModeEnum.Captured;
                return;

            case ScreenLayerMouseHandling.Visible:
                Input.MouseMode = Input.MouseModeEnum.Visible;
                return;
            }
        }

        Input.MouseMode =
            _rootScreen.MouseHandling == ScreenLayerMouseHandling.Captured ?
            Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
    }
}
