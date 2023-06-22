using System;
using CSharp_PG2.Managers.Object;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities.Ghost;

public class Ghost : Figure
{
    public const string ObjectName = "ghost_shaded";

    public Ghost(string name, Vector3 position) : base(GetMesh(), position, name)
    {
    }

    public override void Draw(float deltaTime, Camera camera, Matrix4 projection)
    {
        Vector3 direction = camera.Position - Position;
        direction.Normalize();
        var  rotationAngle = (float)Math.Atan2(direction.X, direction.Z);
        RotateLocaly(rotationAngle+4f, Position);
        
        base.Draw(deltaTime, camera, projection);
    }

    private static Mesh GetMesh()
    {
        return ObjectManager.GetInstance().GetObject(ObjectName).GetMesh();
    }
}