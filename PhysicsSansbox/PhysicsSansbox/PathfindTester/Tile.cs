using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox.PathfindTester;
//-----------------------
internal enum TileState
{
    Open,
    Closed,
    Start,
    Goal,
    Path
}

//-----------------------
internal class Tile
{
    public bool m_dirty = false;
    public TileState m_state = TileState.Open;
}
