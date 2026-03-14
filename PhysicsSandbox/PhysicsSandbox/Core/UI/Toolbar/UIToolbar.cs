using System.Diagnostics;
using PhysicsSandbox.Utils;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI.Toolbar;


public class UIToolbar : UIElement
{
    private static readonly Color c_seperatorColor = Color.LightGray;
    private static readonly float c_seperatorWidthPercent = 0.0025f;
    private static readonly float c_seperatorHeightPercent = 0.9f;
    //Members
    private readonly Vector2Int m_size;
    private readonly Color m_color;    
    private readonly int m_bufferSize; //How much space to leave between elements and the edge of the toolbar
    private int m_seperatorWidth; //Size in pixels of the seperator between elements
    private int m_elementVerticalPadding;
    private int m_elementStartHeight;
    private int m_elementHeight;
    //Sparse array to enable stable indices even when elements are removed
    //Size shouldnt ever be that big anyway so the null checks aren't a big deal
    private List<ToolbarElement> m_elements = new(); 
    //TODO have two lists, one for right side elements and one for left side elements>

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

        //Its all a bit gross and hardcoded but the UIScaler makes it work
        m_seperatorWidth = (int)(m_size.X * c_seperatorWidthPercent);
        m_elementVerticalPadding = (int)(m_size.Y * c_seperatorHeightPercent * 0.25f);
        m_elementStartHeight = Program.c_screenHeight + m_elementVerticalPadding;
        m_elementHeight = (int)(m_size.Y * c_seperatorHeightPercent - m_elementVerticalPadding);
    } 

    //--------------
    public override void Render
    (     
    )
    {     
        Rectangle bounds = new(0, Program.c_screenHeight, m_size.X, m_size.Y);
        Raylib.DrawRectangleRec(bounds, m_color);       

        for (int i = 0; i < m_elements.Count; i++)
        {
            //Skip deleted or disabled elements
            var element = m_elements[i];
            if (element == null || !element.Enabled) 
            {
                continue;   
            }
            
            element.Render();

            // Draw separator if not last element
            if (i < m_elements.Count - 1)
            {
                float rightEdge = element.RenderBounds.X + element.RenderBounds.Width;
                float leftEdge = m_elements[i + 1].RenderBounds.X;
                float mid = (leftEdge - rightEdge) * 0.5f;
                int seperatorPosition = (int)(rightEdge + mid - m_seperatorWidth * 0.5f);

                Rectangle separatorBounds = new(seperatorPosition, m_elementStartHeight, m_seperatorWidth, (int)(m_size.Y * c_seperatorHeightPercent - m_elementVerticalPadding));
                Raylib.DrawRectangleRec(separatorBounds, c_seperatorColor);
            }
        }
    }

    //--------------
    public override void Update
    (
        float i_deltaTime
    )
    {
        for (int i = 0; i < m_elements.Count; i++)
        {
            //Skip deleted or disabled elements
            var element = m_elements[i];
            if (element == null || !element.Enabled) 
            {
                continue;   
            }
            element.Update(i_deltaTime);
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
    public void UpdateElementBounds()
    {
        int x = m_bufferSize;
        foreach (var element in m_elements)
        {
            //Skip deleted or disabled elements
            if (element == null || !element.Enabled) 
            {
                continue;
            }

            int width = element.BaseWidth + m_bufferSize;
            element.RenderBounds = new Rectangle(x, m_elementStartHeight, element.BaseWidth, m_elementHeight);
            x += width;

            DebugUtils.Assert(x <= Program.c_screenWidth, "Toolbar element extends beyond screen width");
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
    
    //-------------
    public ToolbarCheckbox AddCheckbox
    (
        string i_label,
        Func<bool> i_getter,
        Action<bool> i_setter,
        float i_widthMultiplier //Best guess :(
    )
    {
        ToolbarCheckbox checkbox = new((int)(m_size.X * i_widthMultiplier), i_label, i_getter, i_setter);
        AddElement(checkbox);
        return checkbox;
    }
    
    //-------------
    public ToolbarSlider AddSlider
    (
        float i_widthMultiplier,
        float i_minValue,
        float i_maxValue,
        float i_initialValue,
        Action<float> i_onChange,
        string i_label = "",
        float i_textOffsetFraction = 0.25f
    )
    {
        ToolbarSlider slider = new((int)(m_size.X * i_widthMultiplier), i_minValue, i_maxValue, i_initialValue, i_onChange, i_label, i_textOffsetFraction);
        AddElement(slider);
        return slider;
    }

 }

