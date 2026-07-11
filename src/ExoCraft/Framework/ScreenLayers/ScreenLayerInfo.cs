using System.Diagnostics.CodeAnalysis;

namespace ExoCraft.Framework.ScreenLayers;

public record class ScreenLayerInfo
{
    // These fixed screen definitons could benefit from being stored in a
    // resource file instead, but that is left as future work. It should likely
    // be done when submenus are added to the main menu, assuming they aren't
    // just made into overlays.

    public static readonly ScreenLayerInfo GameScreen
        = ("game screen", "res://Scenes/GameScreen.tscn");

    public static readonly ScreenLayerInfo MainMenuScreen
        = ("main menu screen", "res://Scenes/MainMenuScreen.tscn");

    public required string Name { get; init; }
    public required string Path { get; init; }

    [SetsRequiredMembers]
    public ScreenLayerInfo(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public static implicit operator ScreenLayerInfo(
        (string name, string path) tuple) => new(tuple.name, tuple.path);
}
