using System.Numerics;
using PhysicsSandbox.Utils;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI.Toolbar;

public class ToolbarCheckbox : ToolbarElement
{
    private static readonly int c_fontSize = 24;
    private Vector2 m_size = new(20, 20); //Pre scaled size of the checkbox
    private Rectangle m_boxBounds;
    private string m_label;
    private Func<bool> m_getter;
    private Action<bool> m_setter;
    private Color m_selectedColor = Color.SkyBlue;
    private Color m_unselectedColor = Color.LightGray;
    private Color m_labelColor = Color.White;
    public bool Locked { get; set; } = false;

    //-------------
    public ToolbarCheckbox
    (
        int i_baseWidth,
        string i_label,
        Func<bool> i_getter,
        Action<bool> i_setter
    ) : base(i_baseWidth)
    {
        m_label = i_label;
        m_getter = i_getter;
        m_setter = i_setter;

        m_size = UIScaler.ScaleValue(m_size);
    }

    //-------------
    public override void Render()
    {
        base.Render();

        // Draw the label, then an offset, then the box
        Vector2 position = RenderBounds.Position;
        m_boxBounds = new Rectangle(position.X, position.Y, m_size.X, m_size.Y);

        Raylib.DrawRectangleRec(m_boxBounds, m_getter() ? m_selectedColor : m_unselectedColor);
        Raylib.DrawRectangleLinesEx(m_boxBounds, UIScaler.ScaleValue(2), Color.Black);

        position.X += m_boxBounds.Width + UIScaler.ScaleValue(10);
        Raylib.DrawTextEx(FontManager.GetFontForStyle(FontManager.FontStyle.Regular), m_label, position, UIScaler.ScaleValue(c_fontSize), 0, m_labelColor);
    }

    //-------------
    public override void Update
    (
        float i_deltaTime
    )
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && !Locked)
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            if (Raylib.CheckCollisionPointRec(mousePos, m_boxBounds))
            {
                m_setter(!m_getter());
            }
        }
    }
}