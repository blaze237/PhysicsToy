using PhysicsSansbox.Core;
using PhysicsSansbox.TileRender;
using PhysicsSansbox.Utils;
using Raylib_cs;
using System.Numerics;

namespace PhysicsSansbox.PathfindTester;

internal class PathfindWorld : World
{
    //-----------------------
    public PathfindWorld
    (
    )
    {
        for(int i = 0; i < c_gridSize; i++)
        {
            for(int j = 0; j < c_gridSize; j++)
            {
                m_tiles[i, j] = new Tile();
            }
        }
    }

    //-----------------------
    public override Renderer CreateRenderer
    (
    )
    {
        return new TileRenderer(25, Program.c_screenWidth, Program.c_screenHeight);
    }

    //-----------------------
    public override void Init
    (
    )
    {
        for (int i = 0; i < c_gridSize; i++)
        {
            for (int j = 0; j < c_gridSize; j++)
            {
                m_tiles[i, j].m_state = TileState.Open;
                m_tiles[i, j].m_dirty = true;
            }
        }
    }

    //-----------------------
    public override void Destroy
    (
    )
    {

    }

    //-----------------------
    public override void FixedUpdate
    (
        float i_fixedDeltaTime
    )
    {
       
    }

    //-----------------------
    public override void Update
    (
        float i_deltaTime
    )
    {
        switch (m_worldState)
        {
            case WorldState.RouteSelectionStart:
            case WorldState.RouteSelectionGoal:
                UpdateRouteSelection();
                break;
            case WorldState.Pathfinding:
                break;
            case WorldState.Finished:
                break;
        }

        for (int i = 0; i < c_gridSize; i++)
        {
            for(int j = 0; j < c_gridSize; j++)
            {
                if (m_tiles[i, j].m_dirty)
                {
                    //Update renderer tile colours based on tile states
                    ((TileRenderer)m_renderer).TileColours[i, j] = GetColourForTileType(m_tiles[i, j].m_state);
                    m_tiles[i, j].m_dirty = false;
                }
            }
        }
    }

    //-----------------------
    private void UpdateRouteSelection
    (
    )
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            int clickedTileX = -1;
            int clickedTileY = -1;
            GetTileCoordsFromScreenCoords(Raylib.GetMousePosition(), out clickedTileX, out clickedTileY);

            if(m_worldState == WorldState.RouteSelectionStart)
            {
                m_tiles[clickedTileX, clickedTileY].m_state = TileState.Start;
                m_startPos = new Vector2(clickedTileX, clickedTileY);
                m_worldState = WorldState.RouteSelectionGoal;
            }
            else if(m_worldState == WorldState.RouteSelectionGoal)
            {
                m_tiles[clickedTileX, clickedTileY].m_state = TileState.Goal;
                m_goalPos = new Vector2(clickedTileX, clickedTileY);
                m_worldState = WorldState.Pathfinding;
            }
            m_tiles[clickedTileX, clickedTileY].m_dirty = true;
        }
    }

    //-----------------------
    private void UpdatePathfinding
    (
    )
    {
    }

    //-----------------------
    private static Raylib_cs.Color GetColourForTileType
    (
        TileState i_tileState
    )
    {
        switch (i_tileState)
        {
            case TileState.Open:
                return Raylib_cs.Color.LightGray;
            case TileState.Closed:
                return Raylib_cs.Color.Black;
            case TileState.Start:
                return Raylib_cs.Color.Red;
            case TileState.Goal:
                return Raylib_cs.Color.Blue;
            case TileState.Path:
                return Raylib_cs.Color.Green;
            default:
                return Raylib_cs.Color.White;
        }
    }

   

    //-------------------------------
    public void GetTileCoordsFromScreenCoords
    (
        Vector2Int i_screenPos,
        out int o_tileX,
        out int o_tileY
    )
    {
        o_tileX = i_screenPos.X / ((TileRenderer)m_renderer).TileSize;
        o_tileY = i_screenPos.Y / ((TileRenderer)m_renderer).TileSize;
    }

   

    //-----------------------
    private enum WorldState
    {
        RouteSelectionStart,
        RouteSelectionGoal,
        Pathfinding,
        Finished
    }

    //Members
    private const int c_gridSize = 25;
    private List2D<Tile> m_tiles = new List2D<Tile>(c_gridSize, c_gridSize);
    private WorldState m_worldState = WorldState.RouteSelectionStart;
    private Vector2 m_startPos = new Vector2(-1, -1);
    private Vector2 m_goalPos = new Vector2(-1, -1); 
}


//Color[] colours = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta, Color.Black };
//const float changeChance = 0.01f;
//for (int i = 0; i < 25; i++)
//{
//    for (int j = 0; j < 25; j++)
//    {
//        if (Random.Shared.NextSingle() < changeChance)
//        {
//            ((TileRenderer)m_renderer).TileColours[i, j] = colours[Random.Shared.Next(0, colours.Length)];
//        }
//    }
//}