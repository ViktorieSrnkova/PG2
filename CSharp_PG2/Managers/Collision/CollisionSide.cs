using OpenTK.Mathematics;

namespace CSharp_PG2.Managers.Collision;

public enum CollisionSide
{
    Top,
    Bottom,
    Left,
    Right,
    Front,
    Back,
    None
}

public static class CollisionSideExtensions
{
    public static Vector3 GetDirection(CollisionSide side)
    {
        return side switch
        {
            CollisionSide.Top => Vector3.UnitY,
            CollisionSide.Bottom => -Vector3.UnitY,
            CollisionSide.Left => -Vector3.UnitX,
            CollisionSide.Right => Vector3.UnitX,
            CollisionSide.Front => Vector3.UnitZ,
            CollisionSide.Back => -Vector3.UnitZ,
            _ => Vector3.Zero
        };
    }
}