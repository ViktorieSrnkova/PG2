using System;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities.Ghost;

public class GhostSquare : Ghost
{
    public float RotationPeriod { get; set; } = 4.0f;
    public float SideLength { get; set; } = 4.0f;

    private float _elapsedTime;

    public GhostSquare(string name, Vector3 position) : base(name, position)
    {
        IsStatic = true;
    }

    public override void Draw(float deltaTime, Camera camera, Matrix4 projection)
    {
        _elapsedTime += deltaTime;
        if (_elapsedTime > RotationPeriod)
        {
            _elapsedTime -= RotationPeriod; // Reset the elapsed time to start a new rotation
        }

        // Calculate the angle for the figure's position based on the elapsed time
        float angle = (float)(_elapsedTime / RotationPeriod * 2 * Math.PI);

        // Calculate the new position for the figure
        float figureX, figureZ;
        if (angle < MathF.PI / 2)
        {
            figureX = SideLength * angle / (MathF.PI / 2);
            figureZ = 0;
        }
        else if (angle < MathF.PI)
        {
            figureX = SideLength;
            figureZ = SideLength * (angle - MathF.PI / 2) / (MathF.PI / 2);
        }
        else if (angle < 3 * MathF.PI / 2)
        {
            figureX = SideLength * (1 - (angle - MathF.PI) / (MathF.PI / 2));
            figureZ = SideLength;
        }
        else
        {
            figureX = 0;
            figureZ = SideLength * (1 - (angle - 3 * MathF.PI / 2) / (MathF.PI / 2));
        }

        var updatedPosition = new Vector3(figureX, 0, figureZ);
        SetPosition(updatedPosition);
        
        base.Draw(deltaTime, camera, projection);
    }
}