using System;
using CSharp_PG2.Managers.Collision;
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
    
    public CollisionSide Intersects(BoundingBox other)
    {
        return Intersects(other, Vector3.Zero);
    }
    
    public CollisionSide Intersects(BoundingBox otherBox, Vector3 velocity)
    {
        var adjustedPosition = Position + velocity;
        
        // Calculate the boundaries of the two boxes
        float thisLeft = adjustedPosition.X - Width / 2;
        float thisRight = adjustedPosition.X + Width / 2;
        float thisTop = adjustedPosition.Y + Height / 2;
        float thisBottom = adjustedPosition.Y - Height / 2;
        float thisFront = adjustedPosition.Z + Depth / 2;
        float thisBack = adjustedPosition.Z - Depth / 2;

        float otherLeft = otherBox.Position.X - otherBox.Width / 2;
        float otherRight = otherBox.Position.X + otherBox.Width / 2;
        float otherTop = otherBox.Position.Y + otherBox.Height / 2;
        float otherBottom = otherBox.Position.Y - otherBox.Height / 2;
        float otherFront = otherBox.Position.Z + otherBox.Depth / 2;
        float otherBack = otherBox.Position.Z - otherBox.Depth / 2;

        // Check for collision on each side
        bool collides = thisLeft < otherRight &&
                        thisRight > otherLeft &&
                        thisTop >= otherBottom &&
                        thisBottom <= otherTop &&
                        thisFront > otherBack &&
                        thisBack < otherFront;

        // Determine the collision side
        var collisionSide = CollisionSide.None;

        if (collides)
        {
            float xPenetration = Math.Min(thisRight - otherLeft, otherRight - thisLeft);
            float yPenetration = Math.Min(thisTop - otherBottom, otherTop - thisBottom);
            float zPenetration = Math.Min(thisFront - otherBack, otherFront - thisBack);

            float minPenetration = Math.Min(Math.Min(xPenetration, yPenetration), zPenetration);

            if (xPenetration == minPenetration)
                collisionSide = adjustedPosition.X < otherBox.Position.X ? CollisionSide.Left : CollisionSide.Right;
            else if (yPenetration == minPenetration)
                collisionSide = adjustedPosition.Y < otherBox.Position.Y ? CollisionSide.Bottom : CollisionSide.Top;
            else
                collisionSide = adjustedPosition.Z < otherBox.Position.Z ? CollisionSide.Back : CollisionSide.Front;
        }

        return collisionSide;
    }

    public Vector3 DistanceFromBoundingBox(BoundingBox other)
    {
        // Calculate the center position of each bounding box.
        Vector3 positionA = Position + new Vector3(Width / 2f, Height / 2f, Depth / 2f);
        Vector3 positionB = other.Position + new Vector3(other.Width / 2f, other.Height / 2f, other.Depth / 2f);

        // Calculate the distance vector between the two bounding boxes.
        Vector3 distanceVector = positionA - positionB;

        return distanceVector;
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