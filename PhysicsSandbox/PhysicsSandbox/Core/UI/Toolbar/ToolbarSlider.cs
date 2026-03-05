using System;
using Raylib_cs;
using Vector2 = System.Numerics.Vector2;

namespace PhysicsSandbox.Core.UI.Toolbar;

public class ToolbarSlider : ToolbarElement
{
    private static readonly int c_fontSize = 22;

    //Value from 0 to 1
    public float Delta { get; private set; }
    private float m_minValue;
    private float m_maxValue;
    private Action<float> m_onChange;
    private Color m_backgroundColor = Color.LightGray;
    private Color m_fillColor = Color.SkyBlue;
    private bool m_isDragging = false;
    private string m_label;
    
    //----------------
    public ToolbarSlider
    (
        int i_baseWidth,
        float i_minValue,
        float i_maxValue,
        float i_initialValue,
        Action<float> i_onChange,
        string i_label = ""
    )
    : base(i_baseWidth)
    {
        m_minValue = i_minValue;
        m_maxValue = i_maxValue;
        Delta = (i_initialValue - m_minValue) / (m_maxValue - m_minValue);
        m_onChange = i_onChange;
        m_label = i_label;
    }

    //-------------
    public override void Render()
    {        
        // Draw the background
        Raylib.DrawRectangleRec(RenderBounds, m_backgroundColor);
        
        // Draw the fill
        Rectangle fillBounds = new Rectangle(
            RenderBounds.X,
            RenderBounds.Y,
            RenderBounds.Width * Delta,
            RenderBounds.Height
        );
        Raylib.DrawRectangleRec(fillBounds, m_fillColor);
        
        if (!string.IsNullOrEmpty(m_label))
        {
            Vector2 position = RenderBounds.Position;
            position.X += RenderBounds.Width / 4;
            position += new Vector2(0, UIScaler.ScaleValue(0.5f));
            Raylib.DrawTextEx(FontManager.GetFontForStyle(FontManager.FontStyle.Regular), m_label, position, UIScaler.ScaleValue(c_fontSize), 0, Color.White);
        }
        
        base.Render();
    }

    //-------------
    public override void Update
    (
        float i_deltaTime
    )
    {        
        if (Raylib.IsMouseButtonDown(MouseButton.Left) )
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            if (Raylib.CheckCollisionPointRec(mousePos, RenderBounds) || m_isDragging)
            {
                m_isDragging = true;
                // Calculate new value based on mouse position
                float normalizedX = (mousePos.X - RenderBounds.X) / RenderBounds.Width;
                normalizedX = MathF.Max(0f, MathF.Min(1f, normalizedX));
                Delta = normalizedX;
                float newValue = float.Lerp(m_minValue, m_maxValue, normalizedX);
                m_onChange(newValue);
            }
        }
        else
        {
            m_isDragging = false;
        }
    }
}
