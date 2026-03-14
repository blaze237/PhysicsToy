using System;
using Raylib_cs;
using Vector2 = System.Numerics.Vector2;

namespace PhysicsSandbox.Core.UI.Toolbar;

public class ToolbarSlider : ToolbarElement
{
    private static readonly int c_fontSize = 22;

   //Members
    public float Delta { get; private set; }  //Value from 0 to 1
    public float StepSize {get; set;} //When non-zero, snap to steps of this size
    private float m_minValue;
    private float m_maxValue;
    private Action<float> m_onChange;
    private Color m_backgroundColor = Color.LightGray;
    private Color m_fillColor = Color.SkyBlue;
    private bool m_isDragging = false;
    private string m_label;
    private bool m_inverted = false;
    private float m_textOffsetFraction;    

    //----------------
    public ToolbarSlider
    (
        int i_baseWidth,
        float i_minValue,
        float i_maxValue,
        float i_initialValue,
        Action<float> i_onChange,
        string i_label = "",
        float i_textOffsetFraction = 0.25f
    )
    : base(i_baseWidth)
    {
        //This should support small to big and big to small, currently the delta is wrong
        if (i_minValue > i_maxValue && i_initialValue != i_minValue)
        {
            m_inverted = true;
        }
        m_minValue = i_minValue;
        m_maxValue = i_maxValue;
        Delta = MathF.Abs((i_initialValue - m_minValue) / (m_maxValue - m_minValue));
        if (m_inverted)
        {
            Delta = 1 - Delta;
        }
        m_onChange = i_onChange;
        m_label = i_label;
        m_textOffsetFraction = i_textOffsetFraction;
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
            position.X += RenderBounds.Width * m_textOffsetFraction;
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
        if(Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            if (Raylib.CheckCollisionPointRec(mousePos, RenderBounds))
            {
                m_isDragging = true;
            }
        }

        if (Raylib.IsMouseButtonDown(MouseButton.Left) && m_isDragging)
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            // Calculate new value based on mouse position
            float normalizedX = (mousePos.X - RenderBounds.X) / RenderBounds.Width;
            normalizedX = MathF.Max(0f, MathF.Min(1f, normalizedX));
            Delta = normalizedX;
            float newValue = float.Lerp(m_minValue, m_maxValue, normalizedX);
            
            if (StepSize > 0f)
            {
                newValue = MathF.Round(newValue / StepSize) * StepSize;
                newValue = MathF.Max(m_minValue, MathF.Min(m_maxValue, newValue));
            
                // Update Delta to match the snapped value
                normalizedX = (newValue - m_minValue) / (m_maxValue - m_minValue);
                normalizedX = MathF.Max(0f, MathF.Min(1f, normalizedX));
                Delta = normalizedX;
            }
            
            m_onChange(newValue);    
        }
        else
        {
            m_isDragging = false;
        }
    }
}
