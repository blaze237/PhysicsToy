using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox.TileRender
{
    class Tile
    {
        Color[] colours = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta, Color.Black };

        public Tile() 
        {
            Colour = colours[Random.Shared.Next(0, colours.Length)];
            TimeSinceColourChange = Random.Shared.NextSingle() * c_colourChangeTime;
        }

        public void RandomizeColour()
        {
            Colour = colours[Random.Shared.Next(0, colours.Length)];
        }

        public const float c_colourChangeTime = 0.75f;
        public Color Colour { get; set; } = Color.Blue;
        public float TimeSinceColourChange { get; set; }
    }
    class TileRenderer
    {
        //-------------------------------------
        public TileRenderer
        (
            int i_gridSize,
            int i_screenWidth,
            int i_screenHeight
        )
        {
            GridSize = i_gridSize;
            m_tiles = new Tile[GridSize * GridSize];

            for(int i = 0; i < m_tiles.Length; i++)
            {
                m_tiles[i] = new Tile();
            }

            //TODO support non square screens by keeping the grid square and centering it on the screen, for now just assert that the screen is square and that the tile size is an integer
            Debug.Assert(i_screenWidth % i_gridSize == 0, "Screen width must be divisible by size X");
            Debug.Assert(i_screenHeight == i_screenWidth, "Screen height must be equal to screen width");
            TileSize = i_screenWidth / GridSize;

        }


        //--------------------------------------
        public Color GetTileColour
        (
            int x,
            int y
        )
        {
            return m_tiles[Index(x, y)].Colour;
        }

        //-------------------------------
        public void SetTileColour
        (
            int x,
            int y,
            Color colour
        )
        {
            m_tiles[Index(x, y)].Colour = colour;
        }

        //-------------------------------
        public void Render
        (
            float i_dt
        )
        {
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    Raylib.DrawRectangle(x * TileSize, y * TileSize, TileSize, TileSize, GetTileColour(x, y));

                    m_tiles[Index(x, y)].TimeSinceColourChange += i_dt;
                    if (m_tiles[Index(x, y)].TimeSinceColourChange >= Tile.c_colourChangeTime)
                    {
                        m_tiles[Index(x, y)].RandomizeColour();
                        m_tiles[Index(x, y)].TimeSinceColourChange = 0f;
                    }
                }
            }
        }


        //Private methods
        int Index(int x, int y) => y * GridSize + x;

        //Members
        public int GridSize { get; }
        public int TileSize { get; set; }
        Tile[] m_tiles;
        


    }
}
