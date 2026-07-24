using System.Numerics;
using PhysicsSandbox.Utils;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI;

public class TextInput : UIElement
{
    private const int c_boxOutlineWidth = 2;
    private const int c_labelFontSize = 20;
    private const int c_inputBoxHeight = 30;
    
    private string m_label;
    private string m_text;
    private Vector2Int m_position;
    private Vector2Int m_size;
    private Action<string> m_onConfirm;
    private Action m_onCancel;

    //Caret state
    private bool m_caretVisible = true;
    private float m_caretBlinkTimer = 0.0f;
    private const float c_caretBlinkInterval = 0.5f;

    private bool m_maxLengthReached = false;

    //-----------------
    public TextInput
    (
        uint i_id, 
        Action<string> i_onConfirm, 
        Action i_onCancel, 
        Vector2Int i_size, 
        string i_label,
        string i_text = ""
    ) : base(i_id)
    {
        m_onConfirm = i_onConfirm;
        m_onCancel = i_onCancel;
        m_size = i_size;
        m_position = new Vector2Int(Program.c_screenWidth / 2 - i_size.X / 2, Program.c_screenHeight / 2 - i_size.Y / 2);
        m_text = i_text;
        m_label = i_label;
        }

    //-----------------
    public TextInput
    (
        uint i_id, 
        Action<string> i_onConfirm, 
        Action i_onCancel, 
        Vector2Int i_position, 
        Vector2Int i_size, 
        string i_label,
        string i_text = ""
    ) : base(i_id)
    {
        m_onConfirm = i_onConfirm;
        m_onCancel = i_onCancel;
        m_position = i_position;
        m_size = i_size;
        m_text = i_text;
        m_label = i_label;
    }
    
    //-----------------
    public override void Render()
    {
        //Draw a box to contain everything
        Rectangle bounds = new(m_position.X, m_position.Y, m_size.X, m_size.Y);
        UIScaler.ScaleRect(ref bounds);
        Raylib.DrawRectangleRec(bounds, Color.Black);
        Raylib.DrawRectangleLinesEx(bounds, UIScaler.ScaleValue(c_boxOutlineWidth), Color.White);
        
        //Label
        Vector2 position = m_position.ToVector2() + new Vector2(10, 10);
        position = UIScaler.ScaleValue(position);
        Raylib.DrawTextEx(FontManager.c_defaultFontBold, m_label, position, UIScaler.ScaleValue(c_labelFontSize), 0, Color.White);

        //Input Box
        Rectangle inputBox = new(bounds.X + bounds.Width * 0.125f, position.Y + UIScaler.ScaleValue(30), bounds.Width * 0.75f, UIScaler.ScaleValue(c_inputBoxHeight));
        Raylib.DrawRectangleLinesEx(inputBox, UIScaler.ScaleValue(c_boxOutlineWidth/2), Color.White);

        //Text
        Vector2 textPosition = inputBox.Position + new Vector2(UIScaler.ScaleValue(5), UIScaler.ScaleValue(5));
        textPosition = UIScaler.ScaleValue(textPosition);
        Raylib.DrawTextEx(FontManager.c_defaultFont, m_text, textPosition, UIScaler.ScaleValue(c_labelFontSize), 0, Color.LightGray);
        Vector2 textBounds = Raylib.MeasureTextEx(FontManager.c_defaultFont, m_text, UIScaler.ScaleValue(c_labelFontSize), 0.0f);
        DebugUtils.Assert(textBounds.X <= inputBox.Width && textBounds.Y <= inputBox.Height, "Text is too large to fit in input box");
        m_maxLengthReached = textBounds.X >= inputBox.Width;
        
        //Caret
        if(m_caretVisible)
        {
            Vector2 caretPosition = textPosition + new Vector2(textBounds.X + 1, 0);
            Raylib.DrawRectangleRec(new Rectangle(caretPosition.X, caretPosition.Y, UIScaler.ScaleValue(2), UIScaler.ScaleValue(c_inputBoxHeight * 0.75f)), Color.LightGray);
        }
        
        //Confirm text
        Vector2 confirmTextPosition = new(bounds.X + bounds.Width * 0.125f, position.Y + UIScaler.ScaleValue(30) + UIScaler.ScaleValue(c_inputBoxHeight) + UIScaler.ScaleValue(10));
        confirmTextPosition = UIScaler.ScaleValue(confirmTextPosition);
        Raylib.DrawTextEx(FontManager.c_defaultFont, "Press Enter to confirm, Esc to cancel", confirmTextPosition, UIScaler.ScaleValue(c_labelFontSize), 0, Color.White);
   }
    
    //-----------------
    public override void Update
    (
        float i_deltaTime
    )
    {
        if(Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            m_onConfirm(m_text);
            UIManager.Instance.UnregisterElement(ID);
        }
        else if(Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            m_onCancel();
            UIManager.Instance.UnregisterElement(ID);
        }
        else if(Raylib.IsKeyPressed(KeyboardKey.Backspace))
        {
            if(m_text.Length > 0)
            {
                //Doesnt handle unicode characters properly
                m_text = m_text.Substring(0, m_text.Length - 1);
                m_caretBlinkTimer = 0.0f;
            }
        }

        //Handle text input
        int ch = Raylib.GetCharPressed();
        if (ch >= 32 && ch <= 126 && !m_maxLengthReached) //Ignore control characters and extended ASCII
        {
            m_text += (char)ch;
            m_caretBlinkTimer = 0.0f;
        }
        
        m_caretBlinkTimer += i_deltaTime;
        if(m_caretBlinkTimer >= c_caretBlinkInterval)
        {
            m_caretVisible = !m_caretVisible;
            m_caretBlinkTimer = 0.0f;
        }
    }
}
