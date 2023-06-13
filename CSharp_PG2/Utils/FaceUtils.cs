using System;
using System.Collections.Generic;
using System.Linq;
using CSharp_PG2.Managers.Object.Entity;
using CSharp_PG2.Managers.Texture;
using NUnit.Framework;
using OpenTK.Mathematics;

namespace CSharp_PG2.Utils;

public static class FaceUtils
{
    public static VerticesIndices SimplifyFaces(List<Face> faces, List<Vector3> vertices, List<Vector3> normals,
        List<Vector2> textCoords)
    {
        // Loop through faces, add all vertices to a dictionary with key being index and add index to indices list
        // If key already exist, don't add to dictionary, but add index to indices list

        var indices = new List<uint>();
        var keys = new Dictionary<string, uint>();
        var counter = 1;
        var verticesDict = new Dictionary<uint, Indices>();

        foreach (var face in faces)
        {
            for (var i = 0; i < 3; i++)
            {
                var tempIndices = new Indices
                {
                    Position = FloatToUint(face.VertexIndices[i]),
                    Texture = face.TextureIndices.GetAsInt(i),
                    Normal = FloatToUint(face.NormalIndices[i])
                };

                var key = tempIndices.ToString();
                if (!keys.ContainsKey(key))
                {
                    keys.Add(key, (uint)counter-1);
                    verticesDict.Add((uint)counter, tempIndices);
                    indices.Add((uint)counter-1);
                    counter++;
                }
                else
                {
                    indices.Add(keys[key]);
                }
            }
        }

        var resultVertices = new List<Vertex>();
        foreach (var item in verticesDict)
        {
            var value = item.Value;
            resultVertices.Add(new Vertex
            {
                Position = vertices[(int)value.Position -1],
                TexCoord = value.Texture != null ? textCoords[(int)value.Texture -1] : new Vector2(),
                Normal = normals[(int)value.Normal -1]
            });
        }

        return new VerticesIndices
        {
            Vertices = resultVertices.ToArray(),
            Indices = indices.ToArray()
        };
    }

    public static TextureUsage[] GetTextures(List<Face> faces)
    {
        var usages = new List<TextureUsage>();
        string? currentTexture = null;
        var counter = 0;

        foreach (var face in faces)
        {
            if (currentTexture != face.Material?.TextureFile)
            {
                currentTexture = face.Material?.TextureFile;
                if (counter > 0 && currentTexture != null)
                {
                    usages.Add(new TextureUsage
                    {
                        Length = counter,
                        Texture = TextureManager.GetInstance().GetTexture(currentTexture)
                    });
                    counter = 0;
                }
            }
            
            counter++;
        }

        if (currentTexture != null && counter > 0)
        {
            usages.Add(new TextureUsage
            {
                Length = counter,
                Texture = TextureManager.GetInstance().GetTexture(currentTexture)
            });
        }

        return usages.ToArray();
    }
    
    private static int FloatToUint(float f)
    {
        var integer = (int)Math.Ceiling(f);
        return integer;
        // return BitConverter.ToUInt32(BitConverter.GetBytes(f), 0);
    }

    public struct TextureUsage
    {
        public int? Length;
        public Texture? Texture;
    }
    
    public struct VerticesIndices
    {
        public Vertex[] Vertices;
        public uint[] Indices;
    }

    private struct Indices
    {
        public int Position;
        public int? Texture;
        public int Normal;

        public string ToString()
        {
            return $"{Position}/{Texture}/{Normal}";
        }
    }
}