using Godot;

using System.Collections.Generic;

using MouseMode = Godot.Input.MouseModeEnum;

namespace ExoCraft.Globals;

public partial class InteractiveOverlayTracker : Node
{
    public static InteractiveOverlayTracker Instance { get; private set; } = null!;

    public override void _EnterTree()
    {
        if (Instance is not null)
        {
            GD.PushError(
                "VisibleOverlayTracker: Only one instance can be in the " +
                "scene tree at a time.");
            return;
        }

        Instance = this;
    }

    public override void _ExitTree()
    {
        if (Instance == this) Instance = null!;
    }

    public override void _Ready()
    {
        _isReady = true;
        SyncState();
    }

    public int Count { get; private set; } = 0;

    public void TrackUntilExit(CanvasItem overlay)
    {
        if (_overlays.Contains(overlay)) return;
        _overlays.Add(overlay);

        SyncState();

        overlay.TreeExited += () => UnregisterScreen(overlay);
        overlay.VisibilityChanged += SyncState;
    }

    private void UnregisterScreen(CanvasItem overlay)
    {
        if (_overlays.Remove(overlay)) SyncState();
    }

    private void SyncState()
    {
        if (!_isReady) return;

        int count = 0;

        foreach (var overlay in _overlays)
        {
            if (overlay.IsVisibleInTree()) count++;
        }

        Count = count;

        Input.MouseMode = count > 0 ? MouseMode.Visible : MouseMode.Captured;
    }

    private readonly List<CanvasItem> _overlays = new();
    private bool _isReady = false;
}
