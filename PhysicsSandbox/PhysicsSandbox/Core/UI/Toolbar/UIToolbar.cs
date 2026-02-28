using System.Diagnostics;
using System.Net.NetworkInformation;
using PhysicsSandbox.Utils;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI.Toolbar;


//TODO This whole thing is a bit magic numbersy but it will do the job for now
public class UIToolbar : UIElement
{
    //Members
    private static readonly Color c_seperatorColor = Color.LightGray;
    private static readonly float c_seperatorWidthPercent = 0.0025f;
    private static readonly float c_seperatorHeightPercent = 0.9f;
    private readonly Vector2Int m_size;
    private readonly Color m_color;    
    private readonly int m_bufferSize;
    private int m_seperatorWidth;
    private int m_elementVerticalPadding;
    private int m_elementStartHeight;
    private int m_elementHeight;
    //Sparse array to enable stable indices even when elements are removed
    //Size shouldnt ever be that big anyway so the null checks aren't a big deal
    private List<ToolbarElement> m_elements = new();

    
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

        m_seperatorWidth = (int)(m_size.X * c_seperatorWidthPercent);
        m_elementVerticalPadding = (int)(m_size.Y * c_seperatorHeightPercent * 0.25f);
        m_elementStartHeight = Program.c_screenHeight + m_elementVerticalPadding;
        m_elementHeight = (int)(m_size.Y * c_seperatorHeightPercent - m_elementVerticalPadding);

        // AddElement(new ToolbarText((int)(m_size.X * 0.04f), "Test", Color.White));
        // AddElement(new ToolbarText((int)(m_size.X * 0.04f), "Test", Color.Red));
        // AddElement(new ToolbarText((int)(m_size.X * 0.04f), "Test", Color.Green));
        // AddElement(new ToolbarText((int)(m_size.X * 0.04f), "Test", Color.Blue));
        // AddElement(new ToolbarButton((int)(m_size.X * 0.04f), "Test", () => { }));
    } 

    //--------------
    public override void Render
    (     
    )
    {     
        Rectangle bounds = new(0, Program.c_screenHeight, m_size.X, m_size.Y);
        Raylib.DrawRectangleRec(bounds, m_color);
        int seperatorPosition = m_bufferSize;
       

        for (int i = 0; i < m_elements.Count; i++)
        {
            var element = m_elements[i];
            if (element == null) 
            {
                continue;   
            }
            
            element.Render();

            seperatorPosition += element.BaseWidth + m_bufferSize /2;
            // Draw separator if not last element
            if (i < m_elements.Count - 1)
            {
                Rectangle separatorBounds = new(seperatorPosition, m_elementStartHeight, m_seperatorWidth, (int)(m_size.Y * c_seperatorHeightPercent - m_elementVerticalPadding));
                Raylib.DrawRectangleRec(separatorBounds, c_seperatorColor);
            }

            seperatorPosition += m_bufferSize / 2;
        }
    }

    //-------------
    public int AddElement
    (
        ToolbarElement i_element
    ) 
    {
        m_elements.Add(i_element);
        UpdateElementBounds();
        
        return m_elements.Count - 1;
    }

    //-------------
    public void RemoveElement
    (
        int i_index
    )
    {
        m_elements[i_index] = null;
        UpdateElementBounds();
    }

    //-------------
    public void UpdateElementBounds
    (
    )
    {
        int x = m_bufferSize;
        int elementHeight = m_size.Y - m_bufferSize;
        foreach (var element in m_elements)
        {
            if (element == null) 
            {
                continue;
            }

            int width = element.BaseWidth + m_bufferSize;
            element.RenderBounds = new Rectangle(x, m_elementStartHeight, element.BaseWidth, m_elementHeight);
            x += width;

            Debug.Assert(x <= Program.c_screenWidth, "Toolbar element extends beyond screen width");
        }    
    }

    //Factory methods. Elements will be added in order they are called left to right on the toolbar
    //-------------
    public ToolbarText AddText
    (
        string i_text,
        float i_widthMultiplier, //Best guess :(
        Color i_color
    )
    {
        ToolbarText text = new((int)(m_size.X * i_widthMultiplier), i_text, i_color);
        AddElement(text);
        return text;
    }

    //-------------
    public ToolbarButton AddButton
    (
        string i_text,
        float i_widthMultiplier, //Best guess :(
        Action i_onClick
    )
    {
        ToolbarButton button = new((int)(m_size.X * i_widthMultiplier), i_text, i_onClick);
        AddElement(button);
        return button;
    }
    


 }

