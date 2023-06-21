using System;
using System.Collections.Generic;
using CSharp_PG2.Managers.Object.Factory;
using CSharp_PG2.Managers.Shader;
using CSharp_PG2.Utils;
using OpenTK.Mathematics;

namespace CSharp_PG2.Managers.Object.Entity;

public class Figure
{
    public List<Vector3> Vertices { get; set; } = new List<Vector3>();

    public List<Vector3> Normals { get; set; } = new List<Vector3>();

    public List<Vector2> TextureCoordinates { get; set; } = new List<Vector2>();
    
    public List<Face> Faces { get; set; } = new List<Face>();

    public string Shader { get; set; } = "default";
    
    private Mesh? _mesh = null;

    public Mesh GetMesh()
    {
        if (_mesh != null)
        {
            return _mesh;
        }

        var item = FaceUtils.SimplifyFaces(Faces, Vertices, Normals, TextureCoordinates);
        var shader = ShaderManager.GetInstance().GetShader(Shader);

        var textureUsage = FaceUtils.GetTextures(Faces, item.Indices);

        if (shader == null)
        {
            throw new Exception($"Shader '{Shader}' not found");
        }
        
        var minMax = BoundingBoxFactory.GetMinMaxPosition(item.Vertices, item.Indices);
        var vertices = BoundingBoxFactory.GetZeroCenterDiff(minMax, item.Vertices);
        _mesh = new Mesh(shader, vertices, item.Indices);
        _mesh.TextureUsages = new List<FaceUtils.TextureUsage>(textureUsage);

        return _mesh;
    }

    public void Scale(float s)
    {
        for (var i = 0; i < Vertices.Count; i++)
        {
            Vertices[i] *= s;
        }
    }

}