using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace CSharp_PG2;

public class Figure : IDisposable
{
    private readonly Mesh _mesh;

    public Vector3 Position { get; set; } = Vector3.Zero;
    private Matrix4 _model = Matrix4.Identity;
    public bool IsVisible { get; set; } = true;
    private float _currentRotationAngle = 0f;
    
    public Figure(Mesh mesh, Vector3 position)
    {
        _mesh = mesh;
        Move(position);
    }
    
    public void Draw(Camera camera, Matrix4 projection)
    {
        _mesh.Draw(_model, camera.GetViewMatrix(), projection);
    }
    
    public void Move(Vector3 position)
    {
        // _position += position;
        Position += position;
        
        _model *= Matrix4.CreateTranslation(position);
    }
    
    public void SetPosition(Vector3 position)
    {
        Position = position;
        _model = Matrix4.CreateTranslation(position);
    }
    
    public void RotateLocaly(float angle, Vector3 axis)
    {
        Matrix4 translation = Matrix4.CreateTranslation(-axis);
        Matrix4 rotation = Matrix4.CreateRotationY(angle);
        Matrix4 inverseTranslation = Matrix4.CreateTranslation(axis);
        _model *= translation * rotation * inverseTranslation;
    }
    

    public void Dispose()
    {
        _mesh.Dispose();
    }
}