using System;
using PhysicsSandbox.PathfindTester;
using PhysicsSandbox.Utils;

namespace PhysicsSandbox.GraphSolvers.PathfindTester;


public class BFSSolver : GraphSolver
{
    //Members
    private Queue<Vector2Int> m_queue = new Queue<Vector2Int>();


    public BFSSolver
    (
        ref List2D<Tile> i_graph, 
        Vector2Int i_start, 
        Vector2Int i_end, 
        bool i_allowDiag, 
        bool i_randomizeNeighborOrder
    ) : base(ref i_graph, i_start, i_end, i_allowDiag, i_randomizeNeighborOrder)
    {
        m_visited = new List2D<bool>(i_graph.m_width, i_graph.m_height);
        m_parents = new List2D<Vector2Int>(i_graph.m_width, i_graph.m_height);
        m_visited[i_start.X, i_start.Y] = true;
        m_queue.Enqueue(i_start);
    }


    //--------------------
    public override void SolveNextStep()
    {
        Vector2Int current = m_queue.Dequeue();
        if(PreSolveNextStep(current))
        {
            return;
        }
       
        //Solve for the current node by exploring its neighbors
        for(int nIdx = 0; nIdx < m_neighbourIndexOrdering.Length; ++nIdx)
        {
            int shuffledIdx = m_neighbourIndexOrdering[nIdx];
            Vector2Int offset = m_neighbors[shuffledIdx];
            //skip the current node
            if(offset.X == 0 && offset.Y == 0)
            {
                System.Diagnostics.Debug.Assert(false, "Should not reach here");
                continue;
            }
            Vector2Int neighbor = current + offset;
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

            m_visited[neighbor.X, neighbor.Y] = true;
            //Skip neighbors that are blocked
            if(m_graph[neighbor.X, neighbor.Y].State == TileState.Closed)
            {
                continue;
            }
            m_queue.Enqueue(neighbor);
            m_parents[neighbor.X, neighbor.Y] = current;              
            
        }


        //Theres nothing left to visit, we didnt find a path
        if (m_queue.Count == 0)
        {
            Result = GraphSolveResult.NoPathFound;
            return;
        }


    }
}