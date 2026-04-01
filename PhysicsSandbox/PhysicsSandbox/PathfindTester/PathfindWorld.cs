using PhysicsSandbox.Core;
using PhysicsSandbox.Core.UI;
using PhysicsSandbox.Core.UI.Toolbar;
using PhysicsSandbox.GraphSolvers.PathfindTester;
using PhysicsSandbox.PathfindTester.GraphSolvers;
using PhysicsSandbox.TileRender;
using PhysicsSandbox.TileRender.TileRenderer;
using PhysicsSandbox.Utils;
using PhysicsToy.PathfindTester;
using Raylib_cs;
using System.Numerics;
using static PhysicsSandbox.Core.UI.UIText;
using static PhysicsSandbox.PathfindTester.GraphSolvers.SolverFactory;

namespace PhysicsSandbox.PathfindTester;

public class PathfindWorld : World
{
    private const int c_defaultGridSize = 50;
    private const int c_maxBrushRadius = 5;

    // Common grid sizes that divide evenly into screen widths of 500, 1000, 1500 and 2500
    private static readonly int[] c_gridSizeOptions = [10, 25, 50, 100, 125, 250]; 

    // Members
    private int m_gridSize = c_defaultGridSize;
    private List2D<Tile> m_tiles;
    private WorldState m_worldState = WorldState.CreateObstacles;
    private Vector2Int m_startPos = new(-1, -1);
    private Vector2Int m_goalPos = new(-1, -1); 
    private GraphSolver? m_solver;
    private Algorithm m_algorithm = Algorithm.BFS;
    private InfoPanel_PathWorld m_infoPanel = new();
    private bool m_randomNeighbourExploration = true;
    private bool m_diagonalMovement = true;
    private int m_brushRadius = 0; // Radius 0 -> single tile, 1 -> 3x3, 2 -> 5x5, etc.
    private ToolbarSlider m_gridSizeSlider;

    //Toolbar ui elements
    private ToolbarText m_algorithmText;
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
    public PathfindWorld()
    {
        m_tiles = new List2D<Tile>(m_gridSize, m_gridSize);
        for(int i = 0; i < m_gridSize; i++)
        {
            for(int j = 0; j < m_gridSize; j++)
            {
                m_tiles[i, j] = new Tile();
            }
        }

        //Add toolbar elements
        m_randomNeighbourExplorationCheckbox = UIManager.Instance.Toolbar.AddCheckbox("Rand Neighbour", () => m_randomNeighbourExploration, (value) => m_randomNeighbourExploration = value, 0.18f);
        m_diagonalMovementCheckbox = UIManager.Instance.Toolbar.AddCheckbox("Diagonals", () => m_diagonalMovement, (value) => m_diagonalMovement = value, 0.12f);
        UIManager.Instance.Toolbar.AddButton(" Clear", 0.055f, ClearObstacles);     
        UIManager.Instance.Toolbar.AddButton(" Reset", 0.06f, Reset);     
        UIManager.Instance.Toolbar.AddSlider(0.1f, Program.c_fixedTimeStep, 0.00004f, Program.c_fixedTimeStep, (value) => { Program.c_fixedTimeStep = value; }, "Speed");
        //Int slider for brush radius
        ToolbarSlider brushSlider = UIManager.Instance.Toolbar.AddSlider(0.125f, 0, c_maxBrushRadius, 0, (value) => { m_brushRadius = (int)value; }, "Brush Rad", 0.15f);
        brushSlider.StepSize = 1f;
        //Int slider for grid size
        m_gridSizeSlider = UIManager.Instance.Toolbar.AddSlider(0.125f, 0, c_gridSizeOptions.Length -1, 2, (value) => {OnGridSizeChanged((int)value); }, "Grid Size", 0.15f);
        m_gridSizeSlider.StepSize = 1f;
    }
    

    //---------------------------------
    private void OnGridSizeChanged
    (
        int i_newSize
    )
    {
        if(m_worldState != WorldState.CreateObstacles)
        {
            return;
            //Override the slider back to the old value (or disable the slider when pathfind starts/)
            //maintain a more constant border size by scaling the tiles?
        }

        m_gridSize = c_gridSizeOptions[i_newSize];
        m_tiles = new List2D<Tile>(m_gridSize, m_gridSize);
        for(int i = 0; i < m_gridSize; i++)
        {
            for(int j = 0; j < m_gridSize; j++)
            {
                m_tiles[i, j] = new Tile() { State = TileState.Open, m_dirty = true };
            }
        }

        m_renderer = new TileRenderer(m_gridSize, Program.c_screenWidth, Program.c_screenHeight);
    }

    //-----------------------
    public override Renderer CreateRenderer()
    {
        return new TileRenderer(m_gridSize, Program.c_screenWidth, Program.c_screenHeight);
    }

    //-----------------------
    public override void Init()
    {
        for (int i = 0; i < m_gridSize; i++)
        {
            for (int j = 0; j < m_gridSize; j++)
            {
                m_tiles[i, j].State = TileState.Open;
                m_tiles[i, j].m_dirty = true;
            }
        }

        m_infoPanel.Init();
    }

    //-----------------------
    public override void Destroy()
    {
        m_infoPanel.Destroy();
    }

    //-----------------------
    public override void Update
    (
        float i_fixedDeltaTime
    )
    {
        if(Raylib.IsKeyPressed(KeyboardKey.R))
        {
            Reset();
        }

        //Handle algorithm selection (only when not pathfinding)
        if(m_worldState != WorldState.Pathfinding && m_worldState != WorldState.Finished)
        {
           if(Raylib.IsKeyPressed(KeyboardKey.One))
           {
              m_algorithm = Algorithm.BFS;
              m_infoPanel.SetAlgorithmText("BFS", Color.Gold);
           }
           else if(Raylib.IsKeyPressed(KeyboardKey.Two))
           {
              m_algorithm = Algorithm.DFS;
              m_infoPanel.SetAlgorithmText("DFS", Color.Gold);
           }
        //    else if(Raylib.IsKeyPressed(KeyboardKey.Three))
        //    {
        //       m_algorithm = Algorithm.AStar;
        //       m_infoPanel.SetAlgorithmText("A*", Color.Gold);
        //    }
        }

        //Handle world state transitions
        switch (m_worldState)
        {
            case WorldState.CreateObstacles:
                UpdateObstacleCreation();
                break;
            case WorldState.RouteSelectionStart:
            case WorldState.RouteSelectionGoal:
                UpdateRouteSelection();
                break;
            default:
                break;
        }

         //Update tile colours
        for (int i = 0; i < m_gridSize; i++)
        {
            for(int j = 0; j < m_gridSize; j++)
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
    public override void FixedUpdate
    (
        float i_deltaTime
    )
    {
        if(m_worldState == WorldState.Pathfinding)
        {
            UpdatePathfinding(i_deltaTime);
        } 
    }


    //-----------------------
    private void ClearObstacles()
    {
        if(m_worldState == WorldState.Pathfinding)
        {
            return;
        }

        for (int i = 0; i < m_gridSize; i++)
        {
            for (int j = 0; j < m_gridSize; j++)
            {
                if(m_tiles[i,j].State == TileState.Closed)
                {
                    m_tiles[i, j].State = TileState.Open;
                    m_tiles[i, j].m_dirty = true;
                }
            }
        }
    }

    //-----------------------
    private void Reset()
    {
        //Clear pathfinding state but keep obstacles
        for (int i = 0; i < m_gridSize; i++)
        {
            for (int j = 0; j < m_gridSize; j++)
            {
                if(m_tiles[i,j].State != TileState.Open && m_tiles[i,j].State != TileState.Closed)
                {
                    m_tiles[i, j].State = TileState.Open;
                    m_tiles[i, j].m_dirty = true;
                }
            }
        }

        m_startPos = new(-1, -1);
        m_goalPos = new(-1, -1);
        
        //Reset solver
        m_solver = null;
        m_worldState = WorldState.CreateObstacles;

        m_infoPanel.SetStatusText("Obstacles", Color.White);

        m_randomNeighbourExplorationCheckbox.Locked = false;
        m_diagonalMovementCheckbox.Locked = false;
        m_gridSizeSlider.Locked = false;
    }

    //-----------------------
    private void UpdatePathfinding
    (
        float i_deltaTime
    )
    {
        m_solver.SolveNextStep();
        if(m_solver.Result != GraphSolveResult.InProgress)
        {
            m_worldState = WorldState.Finished;
            m_infoPanel.SetStatusText("Done", Color.Green);

            ClearExploredTiles();
        }      
    }

    //-----------------------
    void ClearExploredTiles()
    {
        for (int i = 0; i < m_gridSize; i++)
        {
            for (int j = 0; j < m_gridSize; j++)
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
    private void UpdateRouteSelection()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            GetTileCoordsFromScreenCoords(Raylib.GetMousePosition(), out int clickedTileX, out int clickedTileY);
            if (clickedTileX < 0 || clickedTileX >= m_gridSize || clickedTileY < 0 || clickedTileY >= m_gridSize)
            {
                return;
            }

            if(m_worldState == WorldState.RouteSelectionStart)
            {
                m_tiles[clickedTileX, clickedTileY].State = TileState.Start;
                m_startPos = new Vector2Int(clickedTileX, clickedTileY);
                m_infoPanel.SetStatusText("Choose Goal", Color.White);
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

                m_solver = SolverFactory.CreateSolver(m_algorithm, ref m_tiles, m_startPos, m_goalPos, m_diagonalMovement, m_randomNeighbourExploration);
                m_worldState = WorldState.Pathfinding;
                m_infoPanel.SetStatusText("Solving", Color.SkyBlue);

                m_randomNeighbourExplorationCheckbox.Locked = true;
                m_diagonalMovementCheckbox.Locked = true;
                m_gridSizeSlider.Locked = true;
            }
            m_tiles[clickedTileX, clickedTileY].m_dirty = true;
        }
    }

    //-----------------------
    private void UpdateObstacleCreation()
    {
        bool inputMade = false;
        TileState stateToSet = TileState.Invalid;
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
           inputMade = true;
           stateToSet = TileState.Closed;
        }
        else if (Raylib.IsMouseButtonDown(MouseButton.Right))
        {
           inputMade = true;
           stateToSet = TileState.Open;
        }

        GetTileCoordsFromScreenCoords(Raylib.GetMousePosition(), out int clickedTileX, out int clickedTileY);
        if (clickedTileX < 0 || clickedTileX >= m_gridSize || clickedTileY < 0 || clickedTileY >= m_gridSize)
        {
            return;
        }

        if(inputMade)
        {
            //m_tiles[clickedTileX, clickedTileY].State = stateToSet;
            //m_tiles[clickedTileX, clickedTileY].m_dirty = true;

            for(int x = -m_brushRadius; x <= m_brushRadius; x++)
            {
                for(int y = -m_brushRadius; y <= m_brushRadius; y++)
                {
                    int tileX = clickedTileX + x;
                    int tileY = clickedTileY + y;
                    if(tileX >= 0 && tileX < m_gridSize && tileY >= 0 && tileY < m_gridSize)
                    {
                        m_tiles[tileX, tileY].m_dirty = true;
                        m_tiles[tileX, tileY].State = stateToSet;
                    }
                }
            }
        }

        if(Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            m_worldState = WorldState.RouteSelectionStart;
            m_infoPanel.SetStatusText("Choose Start", Color.White);
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
        //Account for the offset from centering the grid
        var tileRenderer = (TileRenderer)m_renderer;
        o_tileX = (i_screenPos.X - tileRenderer.Offset) / tileRenderer.TileSize;
        o_tileY = (i_screenPos.Y - tileRenderer.Offset) / tileRenderer.TileSize;
    }
}