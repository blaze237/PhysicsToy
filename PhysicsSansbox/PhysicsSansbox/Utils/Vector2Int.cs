using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox.Utils;
internal class Vector2Int
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

    //Members
    public int X { get; set; }
    public int Y { get; set; }  
}
