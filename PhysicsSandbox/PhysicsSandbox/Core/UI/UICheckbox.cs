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
    private bool m_applyScaling = true;

    //--------------
    public UICheckbox
    (
        Vector2Int i_position, //Raw screen position top left anchorred
        string i_label,
        Func<bool> i_getter,
        Action<bool> i_setter,
        bool i_scalePosition = true
    )
    {
        m_label = i_label;
        m_getter = i_getter;
        m_setter = i_setter;
        m_position = i_position;
        m_applyScaling = i_scalePosition;
        m_position = i_position;
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
        bool i_scalePosition = true
    )
    : this(i_position, i_label, i_getter, i_setter, i_scalePosition)
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
        if (m_applyScaling)
        {
            UIScaler.ScaleRect(ref bounds);
        }
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
            float labelOffset = m_applyScaling ? UIScaler.ScaleValue(m_labelOffset) : m_labelOffset;
            float labelSize = m_applyScaling ? UIScaler.ScaleValue(m_labelSize) : m_labelSize;
            Raylib.DrawText(m_label, (int)(bounds.X + bounds.Width + labelOffset), (int)bounds.Y, (int)labelSize, Color.Black);
        }
    }



    public override void Update(float i_deltaTime) 
    {
        //Detect if the user clicked on the checkbox
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            Rectangle bounds = new(m_position.X, m_position.Y, m_size.X, m_size.Y);
            if (m_applyScaling)
            {
                UIScaler.ScaleRect(ref bounds);
            }
            if (Raylib.CheckCollisionPointRec(mousePos, bounds))
            {
                m_setter(!m_getter());
            }
        }
    }
}
