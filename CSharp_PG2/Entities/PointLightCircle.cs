using System;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities;

public class PointLightCircle : PointLight
{

    public float Dir { get; set; } = -1;
    public float Radius { get; set; } = 20;
    
    private double _elapsedTime = 0.0;
    private double _rotationPeriod = 5.0;

    private Vector3 _offset;
    
    public PointLightCircle(string name, int index, Mesh mesh, Vector3 position, Vector3 ambient, Vector3 diffuse, Vector3 specular) : base(name, index, mesh, position, ambient, diffuse, specular)
    {
        IsStatic = true;

        _offset = Position;
    }

    public override void Draw(float deltaTime, Camera camera, Matrix4 projection)
    {
        _elapsedTime += deltaTime;
        if (_elapsedTime > _rotationPeriod)
        {
            _elapsedTime -= _rotationPeriod; // Reset the elapsed time to start a new rotation
        }

        // Calculate the angle for the light's position based on the elapsed time
        var angle = (float)(_elapsedTime / _rotationPeriod * 2 * Math.PI);
    
        angle *= Dir;
        
        // Calculate the new position for the light
        var lightX = Radius * MathF.Cos(angle);
        var lightZ = Radius * MathF.Sin(angle);
        var updatedPosition = _offset + new Vector3(lightX, 0, lightZ );
        SetPosition(updatedPosition);
        
        base.Draw(deltaTime, camera, projection);
    }
    
    public static PointLightCircle CreateCircular(string name, int index, Vector3 position, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            var mesh = GetMesh();
            return new PointLightCircle(name, index, mesh, position, ambient, diffuse, specular);
        }
}