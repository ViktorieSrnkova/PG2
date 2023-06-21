using System;
using CSharp_PG2.Managers.Object.Entity;
using OpenTK.Mathematics;

namespace CSharp_PG2.Managers.Object.Factory;

public static class BoundingBoxFactory
{

    public static BoundingBox CreateBoundingBox(MinMaxPosition pos, Vector3 position)
    {
        var width = pos.Max.X - pos.Min.X;
        var height = pos.Max.Y - pos.Min.Y;
        var depth = pos.Max.Z - pos.Min.Z;
        
        return new BoundingBox(width, height, depth)
        {
            Position = position
        };
    }

    public static Vertex[] GetZeroCenterDiff(MinMaxPosition minMax, Vertex[] vertices)
    {
        Vector3 center = (minMax.Min + minMax.Max) * 0.5f;
        Vector3 diff = -center;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].Position += diff;
        }

        return vertices;
    }

    public static MinMaxPosition GetMinMaxPosition(Mesh mesh)
    {
        Vertex[] vertices = mesh.GetVertices();
        uint[] indices = mesh.GetIndices();
        
        return GetMinMaxPosition(vertices, indices);
    }

    public static MinMaxPosition GetMinMaxPosition(Vertex[] vertices, uint[] indices)
    {

        // Initialize the minimum and maximum coordinates
        Vector3 min = new Vector3(float.MaxValue);
        Vector3 max = new Vector3(float.MinValue);
        
        // Iterate over the vertices to find the minimum and maximum coordinates
        foreach (uint index in indices)
        {
            Vertex vertex = vertices[index];
        
            min.X = Math.Min(min.X, vertex.Position.X);
            min.Y = Math.Min(min.Y, vertex.Position.Y);
            min.Z = Math.Min(min.Z, vertex.Position.Z);
        
            max.X = Math.Max(max.X, vertex.Position.X);
            max.Y = Math.Max(max.Y, vertex.Position.Y);
            max.Z = Math.Max(max.Z, vertex.Position.Z);
        }
        
        return new MinMaxPosition
        {
            Min = min,
            Max = max
        };
    }
    
    public struct MinMaxPosition
    {
        public Vector3 Min;
        public Vector3 Max;
    }
}