using PhysicsSansbox.Core;
using PhysicsSansbox.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox.PathfindTester;
internal interface IGraphSolver
{
    public void Solve(List2D<Tile> i_graph, Vector2Int i_start, Vector2Int i_end, out List<Vector2Int> o_path);    
}
