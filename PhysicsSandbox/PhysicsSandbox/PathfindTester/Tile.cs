namespace PhysicsSandbox.PathfindTester;
//-----------------------
public enum TileState
{
    Open,
    Closed,
    Start,
    Goal,
    Path,
    Explored,
    Active
}

//-----------------------
public class Tile
{
    // Members
    public bool m_dirty = false;
    private TileState m_state;

    // Methods
    public TileState State 
    { 
        get
        {
            return m_state;
        }
        set
        {
            if(m_state != value)
            {
                m_state = value;
                m_dirty = true;
            }
        }
    }
}
