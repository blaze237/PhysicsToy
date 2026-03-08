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
    private static readonly Font m_defaultFont = Raylib.LoadFont("Assets/Roboto-Regular.ttf");
    private static readonly Font m_defaultFontBold = Raylib.LoadFont("Assets/Roboto-Bold.ttf");
    private static readonly Font m_defaultFontItalic = Raylib.LoadFont("Assets/Roboto-Italic.ttf");

    //-------------------
    public static Font GetFontForStyle
    (
        FontStyle i_style
    )
    {
        Font font = m_defaultFont;
        switch (i_style)
        {
            case FontStyle.Bold:
                font = m_defaultFontBold;
                break;
            case FontStyle.Italic:
                font = m_defaultFontItalic;
                break;
        }
        return font;
    }
}