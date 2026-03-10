using PhysicsSandbox.GraphSolvers.PathfindTester;
using PhysicsSandbox.Utils;

namespace PhysicsSandbox.PathfindTester.GraphSolvers;

public static class SolverFactory
{
    public enum Algorithm
    {
        DFS,
        BFS,
        AStar
    }

    public static GraphSolver CreateSolver
    (
        Algorithm i_algorithm,
        ref List2D<Tile> i_graph,
        Vector2Int i_startPos,
        Vector2Int i_goalPos,
        bool i_diagonalMovement,
        bool i_randomNeighbourExploration
    )
    {
        return i_algorithm switch
        {
            Algorithm.DFS => new DFSSolver(ref i_graph, i_startPos, i_goalPos, i_diagonalMovement, i_randomNeighbourExploration),
            Algorithm.BFS => new BFSSolver(ref i_graph, i_startPos, i_goalPos, i_diagonalMovement, i_randomNeighbourExploration),
            _ => throw new ArgumentOutOfRangeException(nameof(i_algorithm), i_algorithm, null)
        };
    }
}
