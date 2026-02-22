using System;
using System.Numerics;
using PhysicsSandbox.Utils;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI;

public class UICheckbox : UIElement
{
    //Members
    private Vector2Int m_size = new(20, 20);
    private int m_labelOffset = 10;
    private int m_labelSize = 20;
    private int m_borderSize = 2;
    private string m_label;
    private Func<bool> m_getter;
    private Action<bool> m_setter;
    private Vector2Int m_position; 
    private Color m_selectedColor = Color.DarkGray;
    private Color m_unselectedColor = Color.LightGray;
    private Color m_labelColor = Color.Black;

    //--------------
    public UICheckbox
    (
        Vector2Int i_position, //Raw screen position top left anchorred
        string i_label,
        Func<bool> i_getter,
        Action<bool> i_setter,
        float i_scale = 1.0f
    )
    {
        m_label = i_label;
        m_getter = i_getter;
        m_setter = i_setter;
        m_position = i_position;
        m_size.X = (int)(i_scale * m_size.X);
        m_size.Y = (int)(i_scale * m_size.Y);
        m_labelSize = (int)(i_scale * m_labelSize);
        m_borderSize = (int)(i_scale * m_borderSize);
        m_labelOffset = (int)(i_scale * m_labelOffset);
    }

    //--------------
    public UICheckbox
    (
        Vector2Int i_position, //Raw screen position top left anchorred
        string i_label,
        Func<bool> i_getter,
        Action<bool> i_setter,
        Color i_selectedColor,
        Color i_unselectedColor,
        Color i_labelColor,
        float i_scale = 1.0f
    )
    : this(i_position, i_label, i_getter, i_setter, i_scale)
    {
        m_selectedColor = i_selectedColor;
        m_unselectedColor = i_unselectedColor;
        m_labelColor = i_labelColor;
    }

    //--------------
    public override void Render
    (
        
    )
    {
        Rectangle bounds = new(m_position.X, m_position.Y, m_size.X, m_size.Y);
        Raylib.DrawRectangleLinesEx(bounds, m_borderSize, Color.Black);
        if (m_getter())
        {
            Raylib.DrawRectangleRec(bounds, m_selectedColor);
        }
        else
        {
            Raylib.DrawRectangleRec(bounds, m_unselectedColor);
        }
        
        if (!string.IsNullOrEmpty(m_label))
        {
            Raylib.DrawText(m_label, (int)(bounds.X + bounds.Width + m_labelOffset), (int)bounds.Y, m_labelSize, Color.Black);
        }
    }



    public override void Update(float i_deltaTime) 
    {
        //Detect if the user clicked on the checkbox
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            Rectangle bounds = new(m_position.X, m_position.Y, m_size.X, m_size.Y);
            if (Raylib.CheckCollisionPointRec(mousePos, bounds))
            {
                m_setter(!m_getter());
            }
        }
    }
}
