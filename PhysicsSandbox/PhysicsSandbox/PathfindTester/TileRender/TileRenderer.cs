using PhysicsSandbox.Core;
using PhysicsSandbox.Utils;
using Raylib_cs;
using System.Diagnostics;

namespace PhysicsSandbox.TileRender.TileRenderer;


class TileRenderer : Renderer
{
    // Members
    public int GridSize { get; }
    public int TileSize { get; set; }
    public int Offset { get; } = 0;
    public List2D<Color> TileColours {get; set;}
     
    //Todo make this a fraction of the screen size
    private readonly int m_borderWidth = UIScaler.ScaleValue(2);

    // Methods
    //-------------------------------------
    public TileRenderer
    (
        int i_gridSize,
        int i_screenWidth,
        int i_screenHeight
    )
    {
        GridSize = i_gridSize;
        TileColours = new List2D<Color>(GridSize, GridSize);

        for (int y = 0; y < GridSize; y++)
        {
            for (int x = 0; x < GridSize; x++)
            {
                TileColours[x, y] = Color.Black;
            }
        }

        DebugUtils.Assert(i_screenHeight == i_screenWidth, "Screen height must be equal to screen width");

        if((i_screenWidth % i_gridSize) != 0)
        {
            //Get the nearest multiple of grid size (round down so it fits)
            int nearestMultiple = (int)Math.Floor(i_screenWidth / (float)i_gridSize);
            TileSize = nearestMultiple;

            //Calculate the dimensions of the grid in pixels
            int gridWidth = nearestMultiple * i_gridSize;

            //Calculate the offset to center the grid
            Offset = (i_screenWidth - gridWidth) / 2;    
        }
        else
        {
            TileSize = i_screenWidth / GridSize;
        }

    }

    //-------------------------------
    public override void RenderImpl
    (
        float i_dt
    )
    {
        for (int y = 0; y < GridSize; y++)
        {
            for (int x = 0; x < GridSize; x++)
            {
                int drawSize = TileSize - m_borderWidth;
                Raylib.DrawRectangle(x * TileSize + Offset, y * TileSize + Offset, drawSize, drawSize, TileColours[x, y]);
            }
        }
    }

}
