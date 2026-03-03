using PhysicsSandbox.Core;
using PhysicsSandbox.Core.UI;
using PhysicsSandbox.Core.UI.Toolbar;
using PhysicsSandbox.TileRender;
using PhysicsSandbox.Utils;
using PhysicsToy.PathfindTester;
using Raylib_cs;
using System.Numerics;
using static PhysicsSandbox.Core.UI.UIText;

namespace PhysicsSandbox.PathfindTester;

public class PathfindWorld : World
{
    // Members
    private const int c_gridSize = 50;
    private const float c_solveTimeStep = 0.1f;
    private float m_solveTimeAccumulator = 0.0f;
    private List2D<Tile> m_tiles = new List2D<Tile>(c_gridSize, c_gridSize);
    private WorldState m_worldState = WorldState.CreateObstacles;
    private Vector2Int m_startPos = new(-1, -1);
    private Vector2Int m_goalPos = new(-1, -1); 
    private DFSSolver? m_dfsSolver;
    private InfoPanel_DFS m_infoPanel = new();
    private bool m_randomNeighbourExploration = false;
    private bool m_diagonalMovement = false;

    //Toolbar ui elements
    private ToolbarText m_statusText;
    private ToolbarCheckbox m_randomNeighbourExplorationCheckbox;
    private ToolbarCheckbox m_diagonalMovementCheckbox;

    //UI elements
    private UIElementID m_infoBoxID;

    private enum WorldState
    {
        Setup,
        CreateObstacles,
        RouteSelectionStart,
        RouteSelectionGoal,
        Pathfinding,
        Finished
    }

    // Methods
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

        UIManager.Instance.Toolbar.AddText("Alg: DFS", 0.075f, Color.White);
        m_randomNeighbourExplorationCheckbox = UIManager.Instance.Toolbar.AddCheckbox("Random Explore", () => m_randomNeighbourExploration, (value) => m_randomNeighbourExploration = value, 0.18f);
        m_diagonalMovementCheckbox = UIManager.Instance.Toolbar.AddCheckbox("Diagonals", () => m_diagonalMovement, (value) => m_diagonalMovement = value, 0.12f);
        m_statusText = UIManager.Instance.Toolbar.AddText("Status: Obstacles", 0.155f, Color.Green);
        UIManager.Instance.Toolbar.AddButton("Reset", 0.05f, () => Reset());
       
    }

    //-----------------------
    public override Renderer CreateRenderer
    (
    )
    {
        return new TileRenderer(c_gridSize, Program.c_screenWidth, Program.c_screenHeight);
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
                m_tiles[i, j].State = TileState.Open;
                m_tiles[i, j].m_dirty = true;
            }
        }

        m_infoPanel.Init(m_randomNeighbourExploration, m_diagonalMovement);
    }

    //-----------------------
    public override void Destroy
    (
    )
    {
        m_infoPanel.Destroy();
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
        if(Raylib.IsKeyPressed(KeyboardKey.R))
        {
            Reset();
        }

        switch (m_worldState)
        {
            case WorldState.CreateObstacles:
                UpdateObstacleCreation();
                break;
            case WorldState.RouteSelectionStart:
            case WorldState.RouteSelectionGoal:
                UpdateRouteSelection();
                break;
            case WorldState.Pathfinding:
                UpdatePathfinding(i_deltaTime);
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
                    ((TileRenderer)m_renderer).TileColours[i, j] = GetColourForTileType(m_tiles[i, j].State);
                    m_tiles[i, j].m_dirty = false;
                }
            }
        }
    }

    //-----------------------
    private void Reset
    (
    )
    {
        
        //Reset tiles to open
        for (int i = 0; i < c_gridSize; i++)
        {
            for (int j = 0; j < c_gridSize; j++)
            {
                m_tiles[i, j].State = TileState.Open;
                m_tiles[i, j].m_dirty = true;
            }
        }

        m_startPos = new(-1, -1);
        m_goalPos = new(-1, -1);
        
        //Reset solver
        m_dfsSolver = null;
        m_worldState = WorldState.CreateObstacles;
        m_solveTimeAccumulator = 0.0f;

        m_statusText.Text = "Status: Obstacles";
        m_statusText.SetBaseWidthMult(0.155f);
        m_statusText.Color = Color.Green;

        m_randomNeighbourExplorationCheckbox.Locked = false;
        m_diagonalMovementCheckbox.Locked = false;
    }

    //-----------------------
    private void UpdatePathfinding
    (
        float i_deltaTime
    )
    {
        m_solveTimeAccumulator += i_deltaTime;
        if(m_solveTimeAccumulator >= c_solveTimeStep)
        {
            m_solveTimeAccumulator = 0.0f;
            m_dfsSolver.SolveNextStep();
            if(m_dfsSolver.Result != GraphSolveResult.InProgress)
            {
                m_worldState = WorldState.Finished;
                m_statusText.Text = "Status: Done";
                m_statusText.SetBaseWidthMult(0.115f);
                m_statusText.Color = Color.Red;

                ClearExploredTiles();
            }
        }
    }

    //-----------------------
    void ClearExploredTiles
    (
    )
    {
        for (int i = 0; i < c_gridSize; i++)
        {
            for (int j = 0; j < c_gridSize; j++)
            {
                if(m_tiles[i, j].State == TileState.Explored)
                {
                    m_tiles[i, j].State = TileState.Open;
                    m_tiles[i, j].m_dirty = true;
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
            GetTileCoordsFromScreenCoords(Raylib.GetMousePosition(), out int clickedTileX, out int clickedTileY);
            if (clickedTileX < 0 || clickedTileX >= c_gridSize || clickedTileY < 0 || clickedTileY >= c_gridSize)
            {
                return;
            }

            if(m_worldState == WorldState.RouteSelectionStart)
            {
                m_tiles[clickedTileX, clickedTileY].State = TileState.Start;
                m_startPos = new Vector2Int(clickedTileX, clickedTileY);
                m_statusText.Text = "Status: Goal";
                m_statusText.SetBaseWidthMult(0.11f);
                m_worldState = WorldState.RouteSelectionGoal;
            }
            else if(m_worldState == WorldState.RouteSelectionGoal)
            {
                if(m_tiles[clickedTileX, clickedTileY].State != TileState.Open)
                {
                    return;
                }
                m_tiles[clickedTileX, clickedTileY].State = TileState.Goal;
                m_goalPos = new Vector2Int(clickedTileX, clickedTileY);

                m_dfsSolver = new DFSSolver(ref m_tiles, m_startPos, m_goalPos, m_diagonalMovement, m_randomNeighbourExploration);
                m_worldState = WorldState.Pathfinding;
                m_statusText.SetBaseWidthMult(0.115f);
                m_statusText.Text = "Status: Solve";

                m_randomNeighbourExplorationCheckbox.Locked = true;
                m_diagonalMovementCheckbox.Locked = true;
            }
            m_tiles[clickedTileX, clickedTileY].m_dirty = true;
        }
    }

    //-----------------------
    private void UpdateObstacleCreation
    (
    )
    {
        GetTileCoordsFromScreenCoords(Raylib.GetMousePosition(), out int clickedTileX, out int clickedTileY);
        if (clickedTileX < 0 || clickedTileX >= c_gridSize || clickedTileY < 0 || clickedTileY >= c_gridSize)
        {
            return;
        }

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
           
            m_tiles[clickedTileX, clickedTileY].State = TileState.Closed;
            m_tiles[clickedTileX, clickedTileY].m_dirty = true;
        }
        else if (Raylib.IsMouseButtonDown(MouseButton.Right))
        {
            m_tiles[clickedTileX, clickedTileY].State = TileState.Open;
            m_tiles[clickedTileX, clickedTileY].m_dirty = true;
        }

        if(Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            m_worldState = WorldState.RouteSelectionStart;
            m_statusText.Text = "Status: Start";
            m_statusText.SetBaseWidthMult(0.115f);
        }
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
            case TileState.Explored:
                return Raylib_cs.Color.Yellow;
            case TileState.Active:
                return Raylib_cs.Color.White;
            default:
                return Raylib_cs.Color.Magenta;
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

   

}