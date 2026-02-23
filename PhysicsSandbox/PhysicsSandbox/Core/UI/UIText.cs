using System.Numerics;
using PhysicsSandbox.Utils;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI;


public class UIText : UIElement
{
    public enum FontStyle
    {
        Regular,
        Bold,
        Italic
    }
    //Members
    private static readonly Font m_defaultFont = Raylib.LoadFont("Assets/Roboto-Regular.ttf");
    private static readonly Font m_defaultFontBold = Raylib.LoadFont("Assets/Roboto-Bold.ttf");
    private static readonly Font m_defaultFontItalic = Raylib.LoadFont("Assets/Roboto-Italic.ttf");
    

    public string Text { get; set; }
    public Color Color { get; set; }
    public FontStyle Style { get; set; }
    public Vector2 Position { get; set; }
    public int FontSize { get; set; }


    //----------
    public UIText
    (
        string i_text,
        Color i_color,
        FontStyle i_style,
        Vector2 i_position,
        int i_fontSize
    )
    {
        Text = i_text;
        Color = i_color;
        Style = i_style;
        Position = i_position;
        FontSize = i_fontSize;
    }
    
    //----------
    public override void Render
    (
    )
    {
        Font font = m_defaultFont;
        switch (Style)
        {
            case FontStyle.Bold:
                font = m_defaultFontBold;
                break;
            case FontStyle.Italic:
                font = m_defaultFontItalic;
                break;
        }
        Raylib.DrawTextEx(font, Text, Position, FontSize, 0, Color);
       
    }
}
