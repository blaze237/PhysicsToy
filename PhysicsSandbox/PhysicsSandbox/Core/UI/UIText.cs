using System.Numerics;
using PhysicsSandbox.Utils;
using Raylib_cs;
using static PhysicsSandbox.Core.UI.FontManager;

namespace PhysicsSandbox.Core.UI;

//TODO move all the scaling to occur in the ui manager not per element


public class UIText : UIElement
{
    
    

    public string Text { get; set; }
    public Color Color { get; set; }
    public FontStyle Style { get; set; }
    public Vector2 Position { get; set; }
    public int FontSize { get; set; }
    public bool ScalePosition { get; set; }

    //----------
    public UIText
    (
        string i_text,
        Color i_color,
        FontStyle i_style,
        Vector2Int i_position,
        int i_fontSize,
        UIElementID i_id,
        bool i_scalePosition = true
    )
    : base(i_id)
    {
        Text = i_text;
        Color = i_color;
        Style = i_style;
        Position = i_position.ToVector2();
        FontSize = i_fontSize;
        ScalePosition = i_scalePosition;
    }
    
    //----------
    public override void Render
    (
    )
    {     
        Vector2 position = ScalePosition ? UIScaler.ScaleValue(Position) : Position;
        Raylib.DrawTextEx(FontManager.GetFontForStyle(Style), Text, position, UIScaler.ScaleValue(FontSize), 0, Color);
       
    }
}
