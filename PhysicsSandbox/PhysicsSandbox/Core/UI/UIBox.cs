using PhysicsSandbox.Utils;
using Raylib_cs;
using RayGui_cs;

namespace PhysicsSandbox.Core.UI;


public class UIBox : UIElement
{
    //Members
    private Vector2Int m_position;
    private Vector2Int m_size;
    private Color m_color = new Color(255, 255, 255, 120);
    private bool m_roundedCorners = false;
    

    //--------------
    public UIBox
    (
        Vector2Int i_position,
        Vector2Int i_size,
        bool i_roundedCorners
    )
    {
        m_position = i_position;
        m_size = i_size;
        m_roundedCorners = i_roundedCorners;
    } 

    //--------------
    public UIBox
    (
        Vector2Int i_position,
        Vector2Int i_size,
        bool i_roundedCorners,
        Color i_color
    )
    {
        m_position = i_position;
        m_size = i_size;
        m_roundedCorners = i_roundedCorners;
        m_color = i_color;
    }
    
    //--------------
    public override void Render
    (     
    )
    {
        .
       Rectangle bounds = new(m_position.X, m_position.Y, m_size.X, m_size.Y);
        if(m_roundedCorners)
        {
            Raylib.DrawRectangleRounded((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height, m_color, 10);
        }
        else
        {
            Raylib.DrawRectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height, m_color);
        }
    }

 }

