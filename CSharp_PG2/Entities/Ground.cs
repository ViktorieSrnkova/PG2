using System.Collections.Generic;
using CSharp_PG2.Managers.Shader;
using CSharp_PG2.Managers.Texture;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities;

public class Ground : Figure
{
    public const string TextureName = "environment:ground";

    public Ground(string name, int size) : base(GetMesh(size), Vector3.Zero, name)
    {
    }

    private static Mesh GetMesh(int size)
    {
        var shader = ShaderManager.GetInstance().GetShader("default");
        var texture = TextureManager.GetInstance().GetTexture(TextureName);

        // Calculate vertices (list of Vertex: Position, Normal, TexCoords)
        var half = size / 2;
        var vertices = new List<Vertex>()
        {
            new Vertex { Position = new Vector3(-half, 0, -half), Normal = Vector3.UnitY, TexCoord = Vector2.Zero },
            new Vertex { Position = new Vector3(-half, 0, half), Normal = Vector3.UnitY, TexCoord = Vector2.UnitY*size },
            new Vertex { Position = new Vector3(half, 0, half), Normal = Vector3.UnitY, TexCoord = Vector2.One*size },
            new Vertex { Position = new Vector3(half, 0, -half), Normal = Vector3.UnitY, TexCoord = Vector2.UnitX*size }
        };
        List<uint> indices = new List<uint>()
        {
            0,1,2,
            0,2,3
        };


        return new Mesh(shader, vertices.ToArray(), indices.ToArray(), texture);
    }
}