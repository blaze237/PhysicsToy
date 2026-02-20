using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox.Utils;
public class Vector2Int
{
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

    //Members
    public int X { get; set; }
    public int Y { get; set; }  
}
