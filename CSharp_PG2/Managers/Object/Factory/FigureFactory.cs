using System.Collections.Generic;
using System.IO;
using System.Text;
using CSharp_PG2.Exceptions.Managers.Object;
using CSharp_PG2.Utils;

namespace CSharp_PG2.Managers.Object.Factory;

using Entity;

public static class FigureFactory
{
    public static Figure FromFile(string path)
    {
        var reader = new StreamReader($"../../../Objects/{path}.obj", Encoding.UTF8);
        
        var obj = new Figure();
        var materials = new Dictionary<string, Material>();
        string? currentMaterial = null;

        while (reader.ReadLine() is { } line)
        {
            var substrings = line.Split(" ");
            switch (substrings[0])
            {
                case "v":
                    var vertex = VertexUtils.FormatVector3FromFile(substrings);
                    if (vertex != null)
                    {
                        obj.Vertices.Add(vertex.Value);
                    }
                    break;
                case "vn":
                    var normal = VertexUtils.FormatVector3FromFile(substrings);
                    if (normal != null)
                    {
                        obj.Normals.Add(normal.Value);
                    }
                    break;
                case "vt":
                    var textureCoordinate = VertexUtils.FormatVector2FromFile(substrings);
                    if (textureCoordinate != null)
                    {
                        obj.TextureCoordinates.Add(textureCoordinate.Value);
                    }
                    break;
                case "f":
                    var face = FaceFactory.FromString(line);
                    if (face != null)
                    {
                        if (currentMaterial != null)
                        {
                            face.Material = materials[currentMaterial];
                        }
                        
                        obj.Faces.Add(face);
                    }
                    break;
                case "mtllib":
                    var materialList = MaterialFactory.FromFile(substrings[1]);
                    foreach (var material in materialList)
                    {
                        materials.Add(material.Name, material);
                    }
                    break;
                case "usemtl":
                    currentMaterial = substrings[1];
                    
                    if (!materials.ContainsKey(currentMaterial))
                    {
                        throw new UnknownMaterialException($"Material {currentMaterial} not found");
                    }
                    
                    break;
            }
        }

        return obj;
    }
}