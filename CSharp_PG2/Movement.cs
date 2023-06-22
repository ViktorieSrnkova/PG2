using System;
using OpenTK.Mathematics;

namespace CSharp_PG2;

public class Movement
{
    private double _elapsedTime = 0.0;
    private double _rotationPeriod = 20.0;
    
    public Vector3 Circle(double res, float y, float dir, float radius)
    {
        _elapsedTime += res;
        if (_elapsedTime > _rotationPeriod)
        {
            _elapsedTime -= _rotationPeriod; // Reset the elapsed time to start a new rotation
        }

        // Calculate the angle for the light's position based on the elapsed time
        float angle = (float)(_elapsedTime / _rotationPeriod * 2 * Math.PI);
    
        angle *= dir;
        // Calculate the new position for the light
        float lightX = radius * MathF.Cos(angle);
        float lightZ = radius * MathF.Sin(angle);
        Vector3 updatedPosition = new Vector3(lightX, y, lightZ );
        return updatedPosition;
    }

    
    

    public Vector3 LineZAxis(double res)
    {
        _rotationPeriod = 40f;
            _elapsedTime += res;
        if (_elapsedTime > _rotationPeriod)
        {
            _elapsedTime -= _rotationPeriod; // Reset the elapsed time to start a new rotation
        }

        // Calculate the angle for the light's position based on the elapsed time
        float angle = (float)(_elapsedTime / _rotationPeriod * 2 * Math.PI);
        float radius = 10.0f; // Adjust this value as needed for the desired radius of the light's circular path

        // Calculate the new position for the light
        float lightZ = radius * MathF.Sin(angle);
        Vector3 updatedPosition = new Vector3(0, 1, lightZ);
        return updatedPosition;
    }

    public Vector3 Square(double res)
    {
        _rotationPeriod = 40f;
        _elapsedTime += res;
        if (_elapsedTime > _rotationPeriod)
        {
            _elapsedTime -= _rotationPeriod; // Reset the elapsed time to start a new rotation
        }

        // Calculate the angle for the figure's position based on the elapsed time
        float angle = (float)(_elapsedTime / _rotationPeriod * 2 * Math.PI);
        float sideLength = 4.0f; // Adjust this value as needed for the desired length of each side of the square

        // Calculate the new position for the figure
        float figureX, figureZ;
        if (angle < MathF.PI / 2)
        {
            figureX = sideLength * angle / (MathF.PI / 2);
            figureZ = 0;
        }
        else if (angle < MathF.PI)
        {
            figureX = sideLength;
            figureZ = sideLength * (angle - MathF.PI / 2) / (MathF.PI / 2);
        }
        else if (angle < 3 * MathF.PI / 2)
        {
            figureX = sideLength * (1 - (angle - MathF.PI) / (MathF.PI / 2));
            figureZ = sideLength;
        }
        else
        {
            figureX = 0;
            figureZ = sideLength * (1 - (angle - 3 * MathF.PI / 2) / (MathF.PI / 2));
        }

        Vector3 updatedPosition = new Vector3(figureX, 0, figureZ);

        return updatedPosition;
    }

    public Vector3 InfinitySymbol(double res)
    {
        _rotationPeriod = 40f;
        _elapsedTime += res;
        if (_elapsedTime > _rotationPeriod)
        {
            _elapsedTime -= _rotationPeriod; // Reset the elapsed time to start a new rotation
        }

        // Calculate the angle for the figure's position based on the elapsed time
        float angle = (float)(_elapsedTime / _rotationPeriod * 2 * Math.PI);
        float radius = 2.0f; // Adjust this value as needed for the desired radius of the figure's path

        // Calculate the new position for the figure using a sinusoidal movement with an hourglass shape
        float figureX = radius * MathF.Sin(angle);
        float figureZ = MathF.Cos(angle) * radius * MathF.Sin(angle);
        float figureY = 0;

        Vector3 updatedPosition = new Vector3(figureX, figureY, figureZ);

        return updatedPosition;
    }
    
}