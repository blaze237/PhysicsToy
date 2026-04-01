namespace PhysicsSandbox.Core.UI;

public class TextInput : UIElement
{
    private string m_label;
    private string m_text;
    private Action<string> m_onConfirm;
    private Action m_onCancel;

    public TextInput(uint i_id, Action<string> i_onConfirm, Action i_onCancel, string i_text = "") : base(i_id)
    {
        m_onConfirm = i_onConfirm;
        m_onCancel = i_onCancel;
        m_text = i_text;
    }

    public override void Render()
    {
       //Draw a box

       //Draw label

       //Draw input box

       //Draw text in the input box

       //Draw caret

       //Draw "Press Enter to confirm, Esc to cancel"


   }
    

    public override void Update(float i_deltaTime)
    {
        if(Raylib_cs.Raylib.IsKeyPressed(Raylib_cs.KeyboardKey.Enter))
        {
            m_onConfirm(m_text);
            UIManager.Instance.UnregisterElement(ID);
        }
        else if(Raylib_cs.Raylib.IsKeyPressed(Raylib_cs.KeyboardKey.Escape))
        {
            m_onCancel();
            UIManager.Instance.UnregisterElement(ID);
        }
    }
}
