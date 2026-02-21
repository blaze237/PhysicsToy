using System;
using PhysicsSansbox.Utils;

namespace PhysicsSansbox.PathfindTester;

public class DFSSolver : GraphSolver
{
    // Members
    private List2D<bool> m_visited;
    private List2D<Vector2Int> m_parents;
    private Stack<Vector2Int> m_stack;   
    private Vector2Int m_lastExploredNode;
    //We use these to define the order in which we explore neighbors. Each pair of values defines a neighbor
    private static readonly int[] m_diagNeighbors = [-1, -1, 0, 1, 1, 1, 0, -1];
    private static readonly int[] m_noDiagNeighbors = [0, 1, 1, 0, 0, -1, -1, 0];
    private int[] m_neighbors;
    //The order in which to visit the neighbour pairs defined above. Stored in an array to allow randomization
    private int[] m_neighbourIndexOrdering = [0, 1, 2, 3];


    // Methods
    //-----------------------
    public DFSSolver
    (
        ref List2D<Tile> i_graph,
        Vector2Int i_start,
        Vector2Int i_end,
        bool i_allowDiag,
        bool i_randomizeNeighborOrder
    ) : base(ref i_graph, i_start, i_end)
    {
        m_visited = new List2D<bool>(i_graph.m_width, i_graph.m_height);
        m_parents = new List2D<Vector2Int>(i_graph.m_width, i_graph.m_height);
        m_stack = new Stack<Vector2Int>();
        m_lastExploredNode = new Vector2Int(-1, -1);   
        m_stack.Push(i_start);

        m_neighbors = i_allowDiag ? m_diagNeighbors : m_noDiagNeighbors;

        if(i_randomizeNeighborOrder)
        {
            m_neighbourIndexOrdering = Enumerable.Range(0, 4).OrderBy(x => Random.Shared.Next()).ToArray();
        }


    }

    //-----------------------
    public override void SolveNextStep
    (
    )
    { 
        //Weve allready either found a path or determined there is no path, no need to keep solving
        if(Result != GraphSolveResult.InProgress)
        {
            return;
        }

        if(m_lastExploredNode.X != -1 && m_lastExploredNode.Y != -1)
        {
            //If the last explored node wasnt the start or end, we can flag it as explored so it gets rendered as such
            if(m_lastExploredNode != m_start && m_lastExploredNode != m_end)
            {
                m_graph[m_lastExploredNode.X, m_lastExploredNode.Y].State = TileState.Explored;
            }
        }

        //I dont get how this is even possible
        if(m_stack.Count == 0)
        {
            Result = GraphSolveResult.NoPathFound;
            return;
        }
        Vector2Int current = m_stack.Pop();
     
        //todo, some gui, controls for the rando and diags

        //Flag the tile as active so it gets rendered as such, but only if its not the start or end tile
        if(current != m_start && current != m_end)
        {
            m_graph[current.X, current.Y].State = TileState.Active;
        }

        m_lastExploredNode = current;

        //we've already visited this node, skip it
        if(m_visited[current.X , current.Y])
        {
            return;
        }

        //This node is blocked, skip it
        if(m_graph[current.X, current.Y].State == TileState.Closed)
        {
            return;
        }

        m_visited[current.X, current.Y] = true;

        //we've reached the end, build the path
        if (current == m_end)
        {
            Vector2Int pathNode = current;
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
            return;
        }

        //Otherwise, we need to explore our neighbours
        for(int nIdx = 0; nIdx < 4; ++nIdx)
        {
            int shuffledIdx = m_neighbourIndexOrdering[nIdx];
            int i = m_neighbors[shuffledIdx * 2];
            int j = m_neighbors[shuffledIdx * 2 + 1];
            //skip the current node
            if(i == 0 && j == 0)
            {
                continue;
            }
            Vector2Int neighbor = new Vector2Int(current.X + i, current.Y + j);
            //Skip neighbors that are out of bounds
            if(neighbor.X < 0 || neighbor.X >= m_graph.m_width || neighbor.Y < 0 || neighbor.Y >= m_graph.m_height)
            {
                continue;
            }
            //Skip neighbors we've already visited
            if(m_visited[neighbor.X, neighbor.Y])
            {
                continue;
            }
            //Skip neighbors that are blocked
            if(m_graph[neighbor.X, neighbor.Y].State == TileState.Closed)
            {
                continue;
            }
            m_stack.Push(neighbor);
            m_parents[neighbor.X, neighbor.Y] = current;              
            
        }


        //Theres nothing left to visit, we didnt find a path
        if (m_stack.Count == 0)
        {
            Result = GraphSolveResult.NoPathFound;
            return;
        }

    }


  
}

