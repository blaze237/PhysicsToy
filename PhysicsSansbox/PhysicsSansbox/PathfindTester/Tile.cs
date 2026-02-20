using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox.PathfindTester;
//-----------------------
public enum TileState
{
    Open,
    Closed,
    Start,
    Goal,
    Path,
    Explored
}

//-----------------------
public class Tile
{
    public bool m_dirty = false;
    private TileState m_state;

    public TileState State 
    { 
        get
        {
            return m_state;
        }
        set
        {
            if(m_state == TileState.Goal || m_state == TileState.Start)
            {
                Console.WriteLine("Attempting to change state of start or goal tile, this shouldnt happen");
                return;
            }
            if(m_state != value)
            {
                m_state = value;
                m_dirty = true;
            }
        }
    }
}
