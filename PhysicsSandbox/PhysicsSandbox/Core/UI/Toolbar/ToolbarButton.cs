using System.Numerics;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI.Toolbar;

public class ToolbarButton : ToolbarElement
{
    //Constants
    private static readonly int c_fontSize = 24;
    
    //Members
    public string Label;
    private Color m_defaultColor;
    private Color m_selectedColor;
    private Color m_hoverColor;
    private Color m_labelColor;
    private Action m_onClick;

    //-------------
    public ToolbarButton
    (
        int i_baseWidth,
        string i_text,
        Color i_defaultColor,
        Color i_selectedColor,
        Color i_hoverColor,
        Color i_labelColor,
        Action i_onClick
    )
    : base(i_baseWidth)
    {
        Label = i_text;
        m_defaultColor = i_defaultColor;
        m_selectedColor = i_selectedColor;
        m_hoverColor = i_hoverColor;
        m_labelColor = i_labelColor;
        m_onClick = i_onClick;
    }

    //-------------
    public ToolbarButton
    (
        int i_baseWidth,
        string i_text,
        Action i_onClick
    )
    : this(i_baseWidth, i_text, Color.Gray, Color.SkyBlue, Color.LightGray, Color.White, i_onClick)
    {
    }

    //-------------
    public override void Render()
    {
        base.Render();
        
        bool isHovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), RenderBounds);
        Color color = isHovered ? m_hoverColor : m_defaultColor;
        if (Raylib.IsMouseButtonDown(MouseButton.Left) && isHovered)
        {
            color = m_selectedColor;
        }
        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && isHovered)
        {
            m_onClick.Invoke();
        }
        
        Raylib.DrawRectangleRec(RenderBounds, color);
        Vector2 position = RenderBounds.Position;
        position += new Vector2(UIScaler.ScaleValue(0.5f), UIScaler.ScaleValue(0.5f));
        Raylib.DrawTextEx(FontManager.GetFontForStyle(FontManager.FontStyle.Regular), Label, position, UIScaler.ScaleValue(c_fontSize), 0, m_labelColor);
    }
}