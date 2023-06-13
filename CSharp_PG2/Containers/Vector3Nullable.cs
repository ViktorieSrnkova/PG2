using System;
using OpenTK.Mathematics;

namespace CSharp_PG2.Containers;

public struct Vector3Nullable
{
    public float? X { get; set; } = null;
    public float? Y { get; set; } = null;
    public float? Z { get; set; } = null;

    public Vector3Nullable(Vector3 vector3)
    {
        X = vector3.X;
        Y = vector3.Y;
        Z = vector3.Z;
    }

    public Vector3Nullable()
    {
    }

    public Vector3Nullable(float? x, float? y, float? z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    // Do the arithmetic operations
    public static Vector3Nullable operator +(Vector3Nullable a, Vector3Nullable b)
    {
        // If both are null, return null
        // If one is null, return the other
        // If both are not null, return the sum

        var c = new Vector3Nullable
        {
            X = SumTwo(a.X, b.X),
            Y = SumTwo(a.Y, b.Y),
            Z = SumTwo(a.Z, b.Z)
        };

        return c;
    }

    public float? this[int index]
    {
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }
        set
        {
            if (index == 0)
            {
                X = value;
            } else if (index == 1)
            {
                Y = value;
            } else if (index == 2)
            {
                Z = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, null);
            }
        }
    }

    public int? GetAsInt(int index)
    {
        var value = this[index];
        if (value == null)
        {
            return null;
        }

        return (int)Math.Ceiling((float)value);
    }

    private static float? SumTwo(float? a, float? b)
    {
        if (a == null && b == null)
        {
            return null;
        }

        if (a == null)
        {
            return b;
        }

        if (b == null)
        {
            return a;
        }

        return a + b;
    }
}