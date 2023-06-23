using System;
using System.Collections.Generic;
using System;
using CSharp_PG2.Entities;
using CSharp_PG2.Entities.Core;
using CSharp_PG2.Managers.Collision;
using CSharp_PG2.Managers.Object.Entity;
using CSharp_PG2.Managers.Object.Factory;
using OpenTK.Mathematics;

namespace CSharp_PG2;

public class Figure : IEntity
{
    private const bool Debug = false;
    
    public bool IsCollidable { get; set; } = true;
    public bool IsStatic { get; set; } = false;
    public Vector3 Velocity { get; set; } = Vector3.Zero;
    public float Weight { get; set; } = 1;
    
    public Vector3 Position { get; set; } = Vector3.Zero;
    public BoundingBox BoundingBox { get; set; }
    protected readonly string Name;
    protected readonly Mesh? Mesh;
    private Quaternion _rotation = Quaternion.Identity;
    protected float Roll = 0;
    private Matrix4 _model = Matrix4.Identity;
    public bool IsVisible { get; set; } = true;
    
    private float _currentRotationAngle = 0f;
    
    public Vector3 BorderColor { get; set; } = new Vector3(220,0,0);
    
    public Figure(Mesh mesh, Vector3 position, string name = "-")
    {
        Mesh = mesh;
        Name = name;
        var minMax = BoundingBoxFactory.GetMinMaxPosition(mesh);
        BoundingBox = BoundingBoxFactory.CreateBoundingBox(minMax, position);
        Move(position);
    }

    public Figure(string name = "-")
    {
        Name = name;
    }

    public virtual void Draw(float deltaTime, Camera camera, Matrix4 projection)
    {
        Mesh?.Draw(_model, camera.GetViewMatrix(), projection);
        if (!Debug) return;
        var boxMesh = BoundingBox.GetMesh();
        var boxShader = boxMesh.GetShader();
        boxShader.Use();
        boxShader.SetVector3("edgeColor", new Vector3(220, 0, 0));

        BoundingBox.GetMesh().Draw(BoundingBox.Model, camera.GetViewMatrix(), projection);
    }

    public void Move(Vector3 position)
    {
        SetPosition(Position + position);
    }

    public Vector3 GetPosition()
    {
        return Position;
    }

    public Vector3 GetDistance(Figure other)
    {
        return BoundingBox.DistanceFromPosition(other.Position);
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;

        var dimensions = BoundingBox.GetDimensions();
        var display = new Vector3(position.X, position.Y + dimensions.Y / 2, position.Z);
        _model = Matrix4.CreateTranslation(display);
        BoundingBox.MoveTo(display);
    }
    
    public void AddVelocity(Vector3 velocity)
    {
        Velocity += velocity;
    }

    public void RotateLocaly(float angle, Vector3 axis)
    {
        Matrix4 translation = Matrix4.CreateTranslation(-axis);
        Matrix4 rotation = Matrix4.CreateRotationY(angle);
        Matrix4 inverseTranslation = Matrix4.CreateTranslation(axis);
        _model *= translation * rotation * inverseTranslation;
    }
    
    public void Rotate(float pitch, float yaw, float roll)
    {
        // Apply rotation logic here
        // Assuming _model is the model to rotate

        // Create a rotation matrix based on the offsets
        var rotation = Quaternion.FromEulerAngles(pitch, -yaw, roll);
        _rotation = Quaternion.Multiply(rotation, _rotation);

        Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(_rotation);
        
        _model *= rotationMatrix;
        BoundingBox.Model *= rotationMatrix;
        

        // Apply the rotation to the model's position
        Position = Vector3.Transform(Position, _rotation);
        BoundingBox.Position = Position;
    }

    public CollisionSide Intersects(Figure other)
    {
        if (BoundingBox == null || other.BoundingBox == null) return CollisionSide.None;
        return BoundingBox.Intersects(other.BoundingBox);
    }
    
    public CollisionSide IntersectsNextFrame(Figure other, Vector3 velocity)
    {
        if (BoundingBox == null || other.BoundingBox == null) return CollisionSide.None;
        return BoundingBox.Intersects(other.BoundingBox, velocity);
    }

    public void Dispose()
    {
        Mesh?.Dispose();
    }

    public string GetName()
    {
        return Name;
    }
}