using System;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI.Toolbar;

public class ToolbarText : ToolbarElement
{
    private static readonly int c_fontSize = 24;
    public string Text { get; set; }
    public Color Color { get; set; }

    public ToolbarText
    (
        int i_baseWidth,
        string i_text,
        Color i_color
    )
    :base(i_baseWidth)
    {
       Text = i_text;
       Color = i_color;
    }

    public override void Render
    (
    )
    {
        base.Render();
        
        //NOTE: We dont really have any way to actually enforce that the text doesnt exceed the bounds of the toolbar element so we just have to hope for the best that sufficient width is allocated
        Raylib.DrawTextEx(FontManager.GetFontForStyle(FontManager.FontStyle.Regular), Text, RenderBounds.Position, UIScaler.ScaleValue(c_fontSize), 0, Color);
    }

  
}
