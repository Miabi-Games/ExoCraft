using ExoCraft.Framework.Math;
using ExoCraft.Framework.VisualWorlds;
using ExoCraft.Pawns;
using ExoCraft.Scenes;

using GdUnit4;

using Godot;

using System;

using static GdUnit4.Assertions;

namespace ExoCraft.Tests.IntegrationTests.Scenes;

[TestSuite]
public class VisualWorldIntegrationTests
{
    [TestCase]
    [RequireGodotRuntime]
    public void SyncingCameraKeepsGodotCameraAtVisualWorldOrigin()
    {
        ISceneRunner runner = ISceneRunner.Load("res://Scenes/VisualWorld.tscn");
        var visualWorld = runner.Scene() as VisualWorld ??
            throw new InvalidOperationException(
                "The visual world scene did not instantiate a VisualWorld root.");
        var mainCamera = visualWorld.GetNode<Camera3D>("MainCamera");
        var rotation = double3basis.identity;
        rotation.rotate_local_x(-System.Math.PI / 6.0);

        visualWorld.CameraTransform = new(
            position: (1000000000000.0, -2000000000000.0, 3000000000000.0),
            rotation,
            scale: 1.0);

        AssertThat(mainCamera.Position).IsEqual(Vector3.Zero);
        AssertThat(mainCamera.Basis.X)
            .IsEqualApprox(ToVector3(rotation.x), Approximation);
        AssertThat(mainCamera.Basis.Y)
            .IsEqualApprox(ToVector3(rotation.y), Approximation);
        AssertThat(mainCamera.Basis.Z)
            .IsEqualApprox(ToVector3(rotation.z), Approximation);
    }

    [TestCase]
    [RequireGodotRuntime]
    public void SyncingPawnSubtractsCameraBeforeConvertingToSinglePrecision()
    {
        ISceneRunner runner = ISceneRunner.Load("res://Scenes/VisualWorld.tscn");
        var visualWorld = runner.Scene() as VisualWorld ??
            throw new InvalidOperationException(
                "The visual world scene did not instantiate a VisualWorld root.");
        var cameraPosition = new double3(
            1000000000000.0,
            -2000000000000.0,
            3000000000000.0);
        var expectedPosition = new double3(1.25, -2.5, 3.75);
        var rotation = double3basis.identity;
        rotation.rotate_local_y(System.Math.PI / 4.0);
        const double scale = 1.5;
        IVisualPawn visualPawn = visualWorld.CreatePlayerPawn();
        var pawnNode = visualPawn as VisualPawn ??
            throw new InvalidOperationException(
                "The player pawn scene did not instantiate a VisualPawn.");

        try
        {
            visualWorld.CameraTransform = new(
                cameraPosition,
                double3basis.identity,
                1.0);
            visualWorld.SyncPawn(
                visualPawn,
                new(
                    cameraPosition + expectedPosition,
                    rotation,
                    scale));

            AssertThat(pawnNode.GlobalPosition)
                .IsEqualApprox(ToVector3(expectedPosition), Approximation);
            AssertThat(pawnNode.GlobalBasis.X)
                .IsEqualApprox(ToVector3(rotation.x * scale), Approximation);
            AssertThat(pawnNode.GlobalBasis.Y)
                .IsEqualApprox(ToVector3(rotation.y * scale), Approximation);
            AssertThat(pawnNode.GlobalBasis.Z)
                .IsEqualApprox(ToVector3(rotation.z * scale), Approximation);
        }
        finally
        {
            visualPawn.Free();
        }
    }

    private static Vector3 ToVector3(double3 value)
        => new((float)value.x, (float)value.y, (float)value.z);

    private static readonly Vector3 Approximation = new(0.00001f, 0.00001f, 0.00001f);
}
