using PhysicsSansbox.Core;
using PhysicsSansbox.Utils;
using Raylib_cs;
using System.Diagnostics;

namespace PhysicsSansbox.TileRender;


class TileRenderer : Renderer
{
    // Members
    public int GridSize { get; }
    public int TileSize { get; set; }
    public List2D<Color> TileColours {get; set;}
    private float m_screenWidth;
    private float m_screenHeight;
     
    //Todo make this a fraction of the screen size
    private const int m_borderWidth = 2;

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
        m_screenHeight = i_screenHeight;
        m_screenWidth = i_screenWidth;

        for (int y = 0; y < GridSize; y++)
        {
            for (int x = 0; x < GridSize; x++)
            {
                TileColours[x, y] = Color.Black;
            }
        }

        //TODO support non square screens by keeping the grid square and centering it on the screen, for now just assert that the screen is square and that the tile size is an integer
        Debug.Assert(i_screenWidth % i_gridSize == 0, "Screen width must be divisible by size X");
        Debug.Assert(i_screenHeight == i_screenWidth, "Screen height must be equal to screen width");
        TileSize = i_screenWidth / GridSize;

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

                int drawSizeX = x < GridSize - 1 ? TileSize - m_borderWidth : TileSize;
                int drawSizeY = y < GridSize - 1 ? TileSize - m_borderWidth : TileSize;
                Raylib.DrawRectangle(x * TileSize, y * TileSize, drawSizeX, drawSizeY, TileColours[x, y]);
            }
        }
    }

}
