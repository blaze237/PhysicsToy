using PhysicsSansbox.Core;
using PhysicsSansbox.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox.PathfindTester;

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

    // Methods
    //-----------------------
    public GraphSolver
    (
        ref List2D<Tile> i_graph,
        Vector2Int i_start, 
        Vector2Int i_end
    )
    {
        m_graph = i_graph;
        m_start = i_start;
        m_end = i_end;
    }
    
    //--------------------------------
    public  List<Vector2Int> GetPath
    (   
    )
    {
        return m_path;
    }

    //-----------------------
    public abstract void SolveNextStep();


}
