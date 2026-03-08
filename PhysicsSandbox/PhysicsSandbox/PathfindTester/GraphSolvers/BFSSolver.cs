using System;
using PhysicsSandbox.PathfindTester;
using PhysicsSandbox.Utils;

namespace PhysicsSandbox.GraphSolvers.PathfindTester;


public class BFSSolver : GraphSolver
{
    public BFSSolver
    (
        ref List2D<Tile> i_graph, 
        Vector2Int i_start, 
        Vector2Int i_end, 
        bool i_allowDiag, 
        bool i_randomizeNeighborOrder
    ) : base(ref i_graph, i_start, i_end)
    {
        
    }


    //--------------------
    public override void SolveNextStep()
    {
        throw new NotImplementedException();
    }
}