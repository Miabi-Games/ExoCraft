using ExoCraft.Framework.Math;
using ExoCraft.Framework.VisualWorlds;

using Godot;

using System.Collections.Generic;

namespace ExoCraft.VoxelTerrain.VoxelChunks;

public partial class VisualVoxelChunk : Node3D, IVisualVoxelChunk
{
    void IVisualVoxelChunk.Free() => QueueFree();

    void IVisualVoxelChunk.SetMesh(
        IReadOnlyList<VoxelVertex> vertices,
        IReadOnlyList<int> indices)
    {
        var positions = new Vector3[vertices.Count];
        var normals = new Vector3[vertices.Count];

        for (int index = 0; index < vertices.Count; index++)
        {
            positions[index] = ToVector3(vertices[index].Position);
            normals[index] = ToVector3(vertices[index].Normal);
        }

        var arrays = new Godot.Collections.Array();
        arrays.Resize((int)Mesh.ArrayType.Max);
        arrays[(int)Mesh.ArrayType.Vertex] = positions;
        arrays[(int)Mesh.ArrayType.Normal] = normals;
        arrays[(int)Mesh.ArrayType.Index] = ToArray(indices);

        var mesh = new ArrayMesh();
        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
        GetNode<MeshInstance3D>("MeshInstance3D").Mesh = mesh;
    }

    void IVisualVoxelChunk.SyncPosition(double3xform position)
    {
        var scale = (float)position.scale;
        var basis = new Basis(
            ToVector3(position.rotation.x) * scale,
            ToVector3(position.rotation.y) * scale,
            ToVector3(position.rotation.z) * scale);

        GlobalTransform = new(basis, ToVector3(position.position));
    }

    private static Vector3 ToVector3(double3 value)
        => new((float)value.x, (float)value.y, (float)value.z);

    private static Vector3 ToVector3(float3 value)
        => new(value.x, value.y, value.z);

    private static int[] ToArray(IReadOnlyList<int> values)
    {
        var result = new int[values.Count];

        for (int index = 0; index < values.Count; index++)
        {
            result[index] = values[index];
        }

        return result;
    }
}
