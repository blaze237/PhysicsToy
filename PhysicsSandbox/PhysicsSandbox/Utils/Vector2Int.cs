using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSandbox.Utils;
public class Vector2Int
{
    // Members
    public int X { get; set; }
    public int Y { get; set; }

    // Methods
    //-----------------------
    public Vector2Int
    (
        int x,
        int y
    )
    {
        X = x;
        Y = y;
    }

    //-----------------------
    public static implicit operator Vector2Int
    (
        Vector2 i_vector
    )
    {
        return new Vector2Int((int)i_vector.X, (int)i_vector.Y);
    }

    //-----------------------
    public static bool operator ==
    (
        Vector2Int a,
        Vector2Int b
    )
    {
        return a.X == b.X && a.Y == b.Y;
    }

    //-----------------------
    public static bool operator !=
    (
        Vector2Int a, 
        Vector2Int b
    )
    {
        return !(a == b);
    }   

    //-----------------------
    public override bool Equals
    (
        object? obj
    )
    {
        if (obj is Vector2Int other)
        {
            return this == other;
        }
        return false;
    }

    //-----------------------
    public override int GetHashCode
    (
    )
    {
        return HashCode.Combine(X, Y);
    }

    //-----------------------
    public static Vector2Int operator +
    (
        Vector2Int a,
        Vector2Int b
    )
    {
        return new Vector2Int(a.X + b.X, a.Y + b.Y);
    }

    //-----------------------
    public static Vector2Int operator -
    (
        Vector2Int a,
        Vector2Int b
    )
    {
        return new Vector2Int(a.X - b.X, a.Y - b.Y);
    }

    //-----------------------
    public static Vector2Int operator *
    (
        Vector2Int a,
        int scalar
    )
    {
        return new Vector2Int(a.X * scalar, a.Y * scalar);
    }

    //-----------------------
    public static Vector2Int operator *
    (
        int scalar,
        Vector2Int a
    )
    {
        return a * scalar;
    }

    //-----------------------
    public static Vector2Int operator /
    (
        Vector2Int a,
        int scalar
    )
    {
        return new Vector2Int(a.X / scalar, a.Y / scalar);
    }

    //-----------------------
    public static Vector2Int operator -
    (
        Vector2Int a
    )
    {
        return new Vector2Int(-a.X, -a.Y);
    }

    //-----------------------
    public Vector2 ToVector2
    (
    )
    {
        return new Vector2(X, Y);
    }
    

  
}
