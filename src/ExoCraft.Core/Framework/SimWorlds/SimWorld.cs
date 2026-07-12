using DefaultEcs;

using System;

namespace ExoCraft.Framework.SimWorlds;

public sealed class SimWorld : IDisposable, ISimWorld
{
    public World EcsWorld { get; }

    public SimWorld()
    {
        EcsWorld = new();
    }

    public void Dispose()
    {
        EcsWorld.Dispose();
    }
}
