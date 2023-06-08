using System;
using System.Collections.Generic;
using System.Globalization;
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

    public static Vector3? FormatVector3FromFile(string[] substrings, int offset = 1)
    {
        if (substrings.Length != 3+offset)
        {
            return null;
        }

        var a = Convert.ToSingle(substrings[1], CultureInfo.InvariantCulture);
        var b = Convert.ToSingle(substrings[2], CultureInfo.InvariantCulture);
        var c = Convert.ToSingle(substrings[3], CultureInfo.InvariantCulture);

        return new Vector3(a, c, c);
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