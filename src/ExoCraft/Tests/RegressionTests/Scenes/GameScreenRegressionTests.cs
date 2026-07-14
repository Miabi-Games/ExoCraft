using ExoCraft.Framework.InputProviders;
using ExoCraft.Framework.Math;
using ExoCraft.Framework.ScreenLayers;
using ExoCraft.Scenes;

using GdUnit4;

using System;
using System.Threading.Tasks;

using static GdUnit4.Assertions;

namespace ExoCraft.Tests.RegressionTests.Scenes;

[TestSuite]
public class GameScreenRegressionTests
{
    [TestCase]
    [RequireGodotRuntime]
    public async Task OpeningMenuDisablesAndClearsGameplayInput()
    {
        ISceneRunner runner = ISceneRunner.Load("res://Scenes/GameScreen.tscn");
        var gameScreen = runner.Scene() as GameScreen ??
            throw new InvalidOperationException(
                "The game screen scene did not instantiate a GameScreen root.");
        var inputProvider = gameScreen.GetNode<InputProvider>("%InputProvider");
        var gameMenu = gameScreen.GetNode<GameMenuOverlay>("%GameMenu");

        try
        {
            runner.SimulateActionPress("move_forward");
            await runner.AwaitInputProcessed();

            AssertThat(inputProvider.MovementInput == float3.zero).IsFalse();

            runner.SimulateActionPressed("ui_cancel");
            await runner.AwaitInputProcessed();

            AssertThat(gameMenu.Visible).IsTrue();
            AssertThat(ScreenLayerManager.OverlayCount).IsEqual(1);
            AssertThat(inputProvider.IsEnabled).IsFalse();
            AssertThat(inputProvider.MovementInput).IsEqual(float3.zero);
        }
        finally
        {
            runner.SimulateActionRelease("move_forward");

            if (ScreenLayerManager.TopOverlay == gameMenu)
            {
                ScreenLayerManager.PopScreenOverlay(
                    nameof(GameScreenRegressionTests), gameMenu);
            }

            await runner.AwaitInputProcessed();
        }
    }
}
