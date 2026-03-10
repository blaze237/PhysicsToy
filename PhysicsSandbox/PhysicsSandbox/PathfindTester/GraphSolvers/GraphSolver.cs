using PhysicsSandbox.Core;
using PhysicsSandbox.PathfindTester;
using PhysicsSandbox.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSandbox.GraphSolvers.PathfindTester;

//-----------------------
public enum GraphSolveResult
{
    InProgress,
    Solved,
    NoPathFound
}

//-----------------------
public abstract class GraphSolver
{
    // Members
    protected List2D<Tile> m_graph;
    protected Vector2Int m_start;
    protected Vector2Int m_end;
    protected List<Vector2Int> m_path = [];
    public GraphSolveResult Result { get; protected set; } = GraphSolveResult.InProgress;
    protected Vector2Int m_lastExploredNode = new(-1, -1);
    protected  List2D<bool> m_visited;
    protected List2D<Vector2Int> m_parents;
    //We use these to define the order in which we explore neighbors. Each pair of values defines a neighbor
    protected static readonly Vector2Int[] m_diagNeighbors = [new(-1, -1), new(0, -1), new(1, -1), new(-1, 0), new(1, 0), new (-1, 1), new (0, 1), new (1, 1)];
    protected static readonly Vector2Int[] m_noDiagNeighbors = [new(0, 1), new(1, 0), new(0, -1), new(-1, 0)];
    protected Vector2Int[] m_neighbors;
    //The order in which to visit the neighbour pairs defined above. Stored in an array to allow randomization
    protected int[] m_nonDiagNeighbourIndexOrdering = [0, 1, 2, 3];
    protected int[] m_diagNeighbourIndexOrdering = [0, 1, 2, 3, 4, 5, 6, 7];
    protected int[] m_neighbourIndexOrdering;

    // Methods
    //-----------------------
    public GraphSolver
    (
        ref List2D<Tile> i_graph,
        Vector2Int i_start, 
        Vector2Int i_end,
        bool i_allowDiag,
        bool i_randomizeNeighborOrder
    )
    {
        m_graph = i_graph;
        m_start = i_start;
        m_end = i_end;
        
        if(i_randomizeNeighborOrder)
        {
            m_diagNeighbourIndexOrdering = Enumerable.Range(0, 8).OrderBy(x => Random.Shared.Next()).ToArray();
            m_nonDiagNeighbourIndexOrdering = Enumerable.Range(0, 4).OrderBy(x => Random.Shared.Next()).ToArray();
        }

        m_neighbors = i_allowDiag ? m_diagNeighbors : m_noDiagNeighbors;
        m_neighbourIndexOrdering = i_allowDiag ? m_diagNeighbourIndexOrdering : m_nonDiagNeighbourIndexOrdering;

        m_visited = new List2D<bool>(i_graph.m_width, i_graph.m_height);
        m_parents = new List2D<Vector2Int>(i_graph.m_width, i_graph.m_height);
    }
    
    //--------------------------------
    public  List<Vector2Int> GetPath
    (   
    )
    {
        return m_path;
    }

    //-----------------------
    //Get ready to solve the next step. Return true if we should skip the current node
    public bool PreSolveNextStep
    (
        in Vector2Int i_current
    )
    {
        //Weve allready either found a path or determined there is no path, no need to keep solving
        if(Result != GraphSolveResult.InProgress)
        {
            return true;
        }

        if(m_lastExploredNode.X != -1 && m_lastExploredNode.Y != -1)
        {
            //If the last explored node wasnt the start or end, we can flag it as explored so it gets rendered as such
            if(m_lastExploredNode != m_start && m_lastExploredNode != m_end)
            {
                m_graph[m_lastExploredNode.X, m_lastExploredNode.Y].State = TileState.Explored;
            }
        }
     
        //Flag the tile as active so it gets rendered as such, but only if its not the start or end tile
        if(i_current != m_start && i_current != m_end)
        {
            m_graph[i_current.X, i_current.Y].State = TileState.Active;
        }

        m_lastExploredNode = i_current;


        //This node is blocked, skip it
        if(m_graph[i_current.X, i_current.Y].State == TileState.Closed)
        {
            return true;
        }

        m_visited[i_current.X, i_current.Y] = true;

        //we've reached the end, build the path
        if (i_current == m_end)
        {
            Vector2Int pathNode = i_current;
            while(pathNode != m_start)
            {
                m_path.Add(pathNode);
                pathNode = m_parents[pathNode.X, pathNode.Y];
                //Flag the tile as part of the path so it gets rendered as such
                if(pathNode != m_start)
                {
                    m_graph[pathNode.X, pathNode.Y].State = TileState.Path;
                }   
            }
            m_path.Reverse();
            Result = GraphSolveResult.Solved;
            return true;
        }


        return false;
    }

    //-----------------------
    public abstract void SolveNextStep();


}
