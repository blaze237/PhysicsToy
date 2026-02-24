using PhysicsSandbox.Utils;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI;


public class UIBox : UIElement
{
    //Members
    private float m_rounding = 0.5f;
    private Vector2Int m_position;
    private Vector2Int m_size;
    private Color m_color;
    private bool m_roundedCorners = false;
    private bool m_scaled = true;
    

    //--------------
    public UIBox
    (
        Vector2Int i_position,
        Vector2Int i_size,
        Color i_color,
        bool i_scaled = true
    )
    {
        m_position = i_position;
        m_size = i_size;
        m_color = i_color;
        m_scaled = i_scaled;
    } 

    //--------------
    public UIBox
    (
        Vector2Int i_position,
        Vector2Int i_size,
        float i_rounding,
        Color i_color,
        bool i_scaled = true
    )
    {
        m_position = i_position;
        m_size = i_size;
        m_roundedCorners = true;
        m_rounding = i_rounding;
        m_color = i_color;
        m_scaled = i_scaled;
    }
    
    //--------------
    public override void Render
    (     
    )
    {     
        Rectangle bounds = new(m_position.X, m_position.Y, m_size.X, m_size.Y);
        if(m_scaled)
        {
            UIScaler.ScaleRect(ref bounds);
        }
        if(m_roundedCorners)
        {
            Raylib.DrawRectangleRounded(bounds, m_rounding, 0, m_color);
        }
        else
        {
            Raylib.DrawRectangleRec(bounds, m_color);
        }
    }

 }

