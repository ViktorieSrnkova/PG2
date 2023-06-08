using System;
using System.Collections.Generic;

namespace CSharp_PG2.Managers.Object;

public class Face
{

    public List<int> VertexIndices { get; set; } = new List<int>();

    public List<int> TextureIndices { get; set; } = new List<int>();
    
    public List<int> NormalIndices { get; set; } = new List<int>();

    public Material? Material { get; set; } = null;
    
    private Face()
    {
    }
    
    public static Face? FromString(string faceString)
    {
        var face = new Face();
        var substrings = faceString.Replace("f ", "").Split(" ");
        try
        {
            foreach (var substring in substrings)
            {
                var indices = substring.Split("/");

                face.VertexIndices.Add(int.Parse(indices[0]));
                if (indices.Length > 1 && !string.IsNullOrEmpty(indices[1]))
                {
                    face.TextureIndices.Add(int.Parse(indices[1]));
                }
                if (indices.Length > 2 && !string.IsNullOrEmpty(indices[2]))
                {
                    face.NormalIndices.Add(int.Parse(indices[2]));
                }
            }
        }
        catch (Exception e)
        {
            return null;
        }

        return face;
    }
    
}