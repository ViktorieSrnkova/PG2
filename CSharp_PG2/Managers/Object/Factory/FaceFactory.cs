using System;
using CSharp_PG2.Containers;
using CSharp_PG2.Managers.Object.Entity;
using OpenTK.Mathematics;

namespace CSharp_PG2.Managers.Object.Factory;

public static class FaceFactory
{
    public static Face? FromString(string faceString)
    {
        var vertex = new Vector3();
        var texture = new Vector3Nullable();
        var normal = new Vector3();
        var substrings = faceString.Replace("f ", "").Split(" ");
        try
        {
            for (int i = 0; i < substrings.Length; i++)
            {
                var indices = substrings[i].Split("/");

                vertex[i] = uint.Parse(indices[0]);
                normal[i] = int.Parse(indices[2]);

                if (indices.Length > 2 && !string.IsNullOrEmpty(indices[1]))
                {
                    texture[i] = int.Parse(indices[1]);
                }
            }
        }
        catch (Exception)
        {
            return null;
        }

        var face = new Face
        {
            VertexIndices = vertex,
            TextureIndices = texture,
            NormalIndices = normal
        };

        return face;
    }
}