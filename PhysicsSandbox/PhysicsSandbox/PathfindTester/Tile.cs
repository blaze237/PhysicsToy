namespace PhysicsSandbox.PathfindTester;
//-----------------------
public enum TileState : byte
{
    Open,
    Closed,
    Start,
    Goal,
    Path,
    Explored,
    Active,

    Invalid
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
