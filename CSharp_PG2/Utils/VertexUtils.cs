using System.Collections.Generic;
using OpenTK.Mathematics;

namespace CSharp_PG2.Utils;

public class VertexUtils
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

}