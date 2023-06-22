using System;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities.Ghost;

public class GhostCircular : Ghost
{
    
    public float Y { get; set; }
    public float Dir { get; set; }
    public float Radius { get; set; }
    
    private double _elapsedTime = 0.0;
    private double _rotationPeriod = 20.0;
    
    public GhostCircular(string name, Vector3 position, float y, float dir, float radius) : base(name, position)
    {
        IsStatic = true;
        Y = y;
        Dir = dir;
        Radius = radius;
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
        var updatedPosition = new Vector3(lightX, Y, lightZ );
        SetPosition(updatedPosition);
        
        base.Draw(deltaTime, camera, projection);
    }
}