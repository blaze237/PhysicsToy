using System.Diagnostics;
using PhysicsSandbox.Core;
using PhysicsSandbox.Utils;

namespace PhysicsSandbox.PathfindTester;

public class GridSerialiser
{
    //--------------------------
    public static void Serialise
    (
        in List2D<Tile> i_grid,
        string i_filePath
    )
    {
        using BinaryWriter writer = new(File.Open(i_filePath, FileMode.Create));
        writer.Write(i_grid.m_width);
        DebugUtils.Assert(i_grid.m_width == i_grid.m_height);
        foreach (Tile tile in i_grid)
        {
            writer.Write((byte)tile.State);
        }
    }
    
    //--------------------------
    public static void Deserialise
    (
        string i_filePath,
        out List2D<Tile> o_grid
    )
    {
        using BinaryReader reader = new(File.Open(i_filePath, FileMode.Open));
        int width = reader.ReadInt32();
        o_grid = new List2D<Tile>(width, width);
        int colInd = 0;
        int rowInd = 0;

        while(rowInd < width)
        {
            o_grid[colInd, rowInd] = new Tile() { State = (TileState)reader.ReadByte(), m_dirty = true };
            colInd++;
            if(colInd >= width)
            {
                colInd = 0;
                rowInd++;
            }
        }
    }
}

