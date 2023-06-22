using System;
using System.Collections.Generic;
using CSharp_PG2.Utils;
using OpenTK.Mathematics;

namespace CSharp_PG2.Managers.Collision;

public class CollisionManager
{
    
    public const float GravityAcceleration = 9.81f;
    public const float AirDragCoefficient = 0.1f;
    public const float PrimaryFriction = 0.9f;
    public const float SecondaryFriction = 1f;
    
    public void Run(float deltaTime, List<Figure> figures)
    {
        var orderedFigures = OrderFiguresByAltitude(figures);
        foreach (var figure in orderedFigures)
        {
            if (figure.IsStatic)
            {
                continue;
            }

            float gravityForce = figure.Weight * GravityAcceleration;

            // Apply vertical velocity based on gravity force
            Vector3 velocity = figure.Velocity;
            velocity.Y -= gravityForce * deltaTime;
            
            foreach (var otherFigure in figures)
            {
                if (otherFigure.GetName() == figure.GetName())
                {
                    continue;
                }

                var collisionSide = figure.IntersectsNextFrame(otherFigure, velocity * deltaTime);
                var newPosition = figure.Position;
                
                var adjustment = collisionSide switch
                {
                    CollisionSide.Top => new Vector3(SecondaryFriction, -PrimaryFriction, SecondaryFriction),
                    CollisionSide.Bottom => new Vector3(SecondaryFriction, -PrimaryFriction, SecondaryFriction),
                    CollisionSide.Left => new Vector3(-PrimaryFriction, SecondaryFriction, SecondaryFriction),
                    CollisionSide.Right => new Vector3(-PrimaryFriction, SecondaryFriction, SecondaryFriction),
                    CollisionSide.Front => new Vector3(SecondaryFriction, SecondaryFriction, -PrimaryFriction),
                    CollisionSide.Back => new Vector3(SecondaryFriction, SecondaryFriction, -PrimaryFriction),
                    CollisionSide.None => Vector3.One,
                    _ => Vector3.One
                };
                
                
                if (collisionSide != CollisionSide.None)
                {
                    velocity *= adjustment;
                }
            }
            
            figure.Velocity = velocity;
            figure.Move(velocity*deltaTime);
        }
    }

    private Vector3 SetGravityVelocity(Vector3 velocity, float gravityForce)
    {
        var gravityVelocity = new Vector3(velocity);
        
        // Only add gravity force if axis velocity is already moving
        if (velocity.X != 0)
        {
            gravityVelocity.X += gravityForce;
        }
        
        if (velocity.Y != 0)
        {
            gravityVelocity.Y += gravityForce;
        }
        
        if (velocity.Z != 0)
        {
            gravityVelocity.Z += gravityForce;
        }
        
        return gravityVelocity;
    }
    
    private Vector3 MultiplyIfExists(Vector3 velocity, Vector3 adjustment)
    {
        var gravityVelocity = new Vector3(velocity);
        
        // Only add gravity force if axis velocity is already moving
        if (velocity.X != 0)
        {
            gravityVelocity.X *= adjustment.X;
        }
        
        if (velocity.Y != 0)
        {
            gravityVelocity.Y *= adjustment.Y;
        }
        
        if (velocity.Z != 0)
        {
            gravityVelocity.Z *= adjustment.Z;
        }
        
        return gravityVelocity;
    }
    
    private List<Figure> OrderFiguresByAltitude(List<Figure> figures)
    {
        var sortedList = new List<Figure>(figures);
        
        sortedList.Sort((figure1, figure2) => figure1.Position.Y.CompareTo(figure2.Position.Y));

        return sortedList;
    }
}