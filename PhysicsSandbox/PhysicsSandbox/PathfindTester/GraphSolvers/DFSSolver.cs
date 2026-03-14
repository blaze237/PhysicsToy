using System;
using PhysicsSandbox.Core;
using PhysicsSandbox.PathfindTester;
using PhysicsSandbox.Utils;

namespace PhysicsSandbox.GraphSolvers.PathfindTester;

public class DFSSolver : GraphSolver
{
    // Members
    private Stack<Vector2Int> m_stack;   
   

    // Methods
    //-----------------------
    public DFSSolver
    (
        ref List2D<PhysicsSandbox.PathfindTester.Tile> i_graph,
        Vector2Int i_start,
        Vector2Int i_end,
        bool i_allowDiag,
        bool i_randomizeNeighborOrder
    ) : base(ref i_graph, i_start, i_end, i_allowDiag, i_randomizeNeighborOrder)
    {
        m_stack = new Stack<Vector2Int>();
        m_stack.Push(i_start);
    }

    //-----------------------
    public override void SolveNextStep
    (
    )
    { 
        //Stack empty means all reachable nodes explored without reaching goal (e.g., goal walled off)
        if(m_stack.Count == 0)
        {
            Result = GraphSolveResult.NoPathFound;
            return;
        }
        
        Vector2Int current = m_stack.Pop();
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
                DebugUtils.Assert(false, "Should not reach here");
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