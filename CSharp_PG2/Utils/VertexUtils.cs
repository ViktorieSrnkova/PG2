using System;
using System.Collections.Generic;
using System.Globalization;
using CSharp_PG2.Managers.Object.Entity;
using OpenTK.Mathematics;

namespace CSharp_PG2.Utils;

public static class VertexUtils
{

    public static Vertex[] ConvertToVertices(float[] original)
    {
        var vertices = new List<Vertex>();
        
        for (int i = 0; i < original.Length; i += 8)
        {
            vertices.Add(new Vertex
            {
                Position = new Vector3(original[i], original[i + 1], original[i + 2]),
                Normal = new Vector3(original[i + 3], original[i + 4], original[i + 5]),
                TexCoord = new Vector2(original[i + 6], original[i + 7]),
            });
        }
        
        return vertices.ToArray();
    }

    public static int[] Vector3ToIntArray(Vector3 vector)
    {
        return new int[3]{(int) vector.X, (int) vector.Y, (int) vector.Z};
    }
    
    // public static Vertex[] ConvertToVertices(List<Face> faces, List<Vector3> vertices, List<Vector3> normals,
    //     List<Vector2> texCoords)
    // {
    //     var vertexList = new List<Vertex>();
    //
    //     for (int i = 0; i < vertices.Count; i++)
    //     {
    //         vertexList.Add();
    //     }
    //     
    //     return vertexList.ToArray();
    // }
    
    public static uint[] ParseFacesToIndices(Face[] faces)
    {
        var indices = new List<uint>();
        
        foreach (var face in faces)
        {
            indices.Add((uint) face.VertexIndices.X);
            indices.Add((uint) face.VertexIndices.Y);
            indices.Add((uint) face.VertexIndices.Z);
        }
        
        return indices.ToArray();
    }
    
    public static Vector3 GetRandomVector3(float min, float max)
    {
        var random = new Random();
        return new Vector3(
            (float) random.NextDouble() * (max - min) + min,
            (float) random.NextDouble() * (max - min) + min,
            (float) random.NextDouble() * (max - min) + min
        );
    }
    
    public static Vector3? FormatVector3FromFile(string[] substrings, int offset = 1)
    {
        if (substrings.Length != 3+offset)
        {
            return null;
        }

        var a = Convert.ToSingle(substrings[1], CultureInfo.InvariantCulture);
        var b = Convert.ToSingle(substrings[2], CultureInfo.InvariantCulture);
        var c = Convert.ToSingle(substrings[3], CultureInfo.InvariantCulture);

        return new Vector3(a, b, c);
    }
    
    public static Vector2? FormatVector2FromFile(string[] substrings, int offset = 1)
    {
        if (substrings.Length != 2+offset)
        {
            return null;
        }

        var a = Convert.ToSingle(substrings[1], CultureInfo.InvariantCulture);
        var b = Convert.ToSingle(substrings[2], CultureInfo.InvariantCulture);

        return new Vector2(a, b);
    }

}