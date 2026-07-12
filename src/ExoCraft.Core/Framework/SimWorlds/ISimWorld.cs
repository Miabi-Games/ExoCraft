using DefaultEcs;

namespace ExoCraft.Framework.SimWorlds;

public interface ISimWorld
{
    World EcsWorld { get; }
}