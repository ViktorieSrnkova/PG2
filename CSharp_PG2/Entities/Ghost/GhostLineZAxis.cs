using System;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities.Ghost;

public class GhostLineZAxis : Ghost
{
    public float RotationPeriod { get; set; } = 4.0f;
    public float Radius { get; set; } = 10.0f;

    private float _elapsedTime;
    
    public GhostLineZAxis(string name, Vector3 position) : base(name, position)
    {
    }

    public override void Draw(float deltaTime, Camera camera, Matrix4 projection)
    {
        RotationPeriod = 40f;
        _elapsedTime += deltaTime;
        if (_elapsedTime > RotationPeriod)
        {
            _elapsedTime -= RotationPeriod; // Reset the elapsed time to start a new rotation
        }

        // Calculate the angle for the light's position based on the elapsed time
        var angle = (float)(_elapsedTime / RotationPeriod * 2 * Math.PI);

        // Calculate the new position for the light
        var lightZ = Radius * MathF.Sin(angle);
        var updatedPosition = new Vector3(0, 1, lightZ);
        
        SetPosition(updatedPosition);
        
        base.Draw(deltaTime, camera, projection);
    }
}