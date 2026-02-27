using System.Diagnostics;
using PhysicsSandbox.Utils;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI.Toolbar;


public class UIToolbar : UIElement
{
    //Members
    private readonly Vector2Int m_size;
    private readonly Color m_color;    
    private readonly int m_bufferSize;
    private Dictionary<UIElementID, ToolbarElement> m_elements = new();


    //--------------
    public UIToolbar
    (
        Vector2Int i_size,
        Color i_color,
        int i_bufferSize,
        UIElementID i_id
    )
    : base(i_id)
    {
        m_size = i_size;
        m_color = i_color;
        m_bufferSize = i_bufferSize;
    } 

    //--------------
    public override void Render
    (     
    )
    {     
        Rectangle bounds = new(0, Program.c_screenHeight, m_size.X, m_size.Y);
        Raylib.DrawRectangleRec(bounds, m_color);
    }

    //-------------
    public void AddElement
    (
        ToolbarElement i_element
    )
    {
        m_elements.Add(i_element.ID, i_element);
        UpdateElementBounds();

    }

    //-------------
    public void RemoveElement
    (
        UIElementID i_id
    )
    {
        m_elements.Remove(i_id);
        UpdateElementBounds();
    }

    private void UpdateElementBounds
    (
    )
    {
        int x = m_bufferSize;
        foreach (var element in m_elements)
        {
            int width = element.Value.BaseWidth + m_bufferSize;
            element.Value.RenderBounds = new Rectangle(x, Program.c_screenHeight, width, m_size.Y);
            x += width;

            Debug.Assert(x <= Program.c_screenWidth, "Toolbar element extends beyond screen width");
        }
        
    }

 }

