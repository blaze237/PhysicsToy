using Raylib_cs;

namespace PhysicsSandbox.Core.UI;

public class FontManager
{
    public enum FontStyle
    {
        Regular,
        Bold,
        Italic
    }
    //Members
    public static readonly Font c_defaultFont = Raylib.LoadFont("Assets/Roboto-Regular.ttf");
    public static readonly Font c_defaultFontBold = Raylib.LoadFont("Assets/Roboto-Bold.ttf");
    public static readonly Font c_defaultFontItalic = Raylib.LoadFont("Assets/Roboto-Italic.ttf");

    //-------------------
    public static Font GetFontForStyle
    (
        FontStyle i_style //Todo: Add more font family
    )
    {
        Font font = c_defaultFont;
        switch (i_style)
        {
            case FontStyle.Bold:
                font = c_defaultFontBold;
                break;
            case FontStyle.Italic:
                font = c_defaultFontItalic;
                break;
        }
        return font;
    }
}