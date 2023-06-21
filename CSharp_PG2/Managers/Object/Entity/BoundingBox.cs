using System;
using CSharp_PG2.Managers.Shader;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;

namespace CSharp_PG2.Managers.Object.Entity;

public class BoundingBox
{
    public Matrix4 Model = Matrix4.Identity; 

    public float Width { get; set; }
    public float Height { get; set; }
    public float Depth { get; set; }

    public Vector3 Position { get; set; } = Vector3.Zero;

    private Mesh? _mesh;
    
    public BoundingBox(float width, float height, float depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }
    
    public bool Intersects(BoundingBox other)
    {
        var distance = DistanceFromPosition(other.Position);
        return distance.X < Width / 2 + other.Width / 2 &&
               distance.Y < Height / 2 + other.Height / 2 &&
               distance.Z < Depth / 2 + other.Depth / 2;
    }

    public Vector3 DistanceFromPosition(Vector3 position)
    {
        var distanceX = Math.Abs(position.X - Position.X) - Width / 2;
        var distanceY = Math.Abs(position.Y - Position.Y) - Height / 2;
        var distanceZ = Math.Abs(position.Z - Position.Z) - Depth / 2;
        
        return new Vector3(distanceX, distanceY, distanceZ);
    }
    
    public void MoveTo(Vector3 position)
    {
        Position = position;
        Model = Matrix4.CreateTranslation(position);
    }
    
    public Mesh GetMesh()
    {
        if (_mesh != null) return _mesh;
        
        // Vertex[] vertices = new Vertex[8]
        // {
        //     new Vertex { Position = new Vector3(Min.X, Min.Y, Min.Z) },
        //     new Vertex { Position = new Vector3(Min.X, Min.Y, Max.Z) },
        //     new Vertex { Position = new Vector3(Max.X, Min.Y, Max.Z) },
        //     new Vertex { Position = new Vector3(Max.X, Min.Y, Min.Z) },
        //     new Vertex { Position = new Vector3(Min.X, Max.Y, Min.Z) },
        //     new Vertex { Position = new Vector3(Min.X, Max.Y, Max.Z) },
        //     new Vertex { Position = new Vector3(Max.X, Max.Y, Max.Z) },
        //     new Vertex { Position = new Vector3(Max.X, Max.Y, Min.Z) }
        // };
        
        Vertex[] vertices = new Vertex[8]
        {
            new Vertex { Position = new Vector3(-Width / 2, -Height / 2, -Depth / 2) },
            new Vertex { Position = new Vector3(-Width / 2, -Height / 2, Depth / 2) },
            new Vertex { Position = new Vector3(Width / 2, -Height / 2, Depth / 2) },
            new Vertex { Position = new Vector3(Width / 2, -Height / 2, -Depth / 2) },
            new Vertex { Position = new Vector3(-Width / 2, Height / 2, -Depth / 2) },
            new Vertex { Position = new Vector3(-Width / 2, Height / 2, Depth / 2) },
            new Vertex { Position = new Vector3(Width / 2, Height / 2, Depth / 2) },
            new Vertex { Position = new Vector3(Width / 2, Height / 2, -Depth / 2) }
        };

        uint[] indices = new uint[24]
        {
            0, 1, 1, 2, 2, 3, 3, 0, // Bottom face
            4, 5, 5, 6, 6, 7, 7, 4, // Top face
            0, 4, 1, 5, 2, 6, 3, 7  // Vertical edges
        };

        var shader = ShaderManager.GetInstance().GetShader("bounding");
        _mesh = new Mesh(shader, vertices, indices, primitiveType: (int) PrimitiveType.Lines);
        return _mesh;
    }
    
    public Vector3 GetDimensions()
    {
        return new Vector3(Width, Height, Depth);
    }
    
    
}