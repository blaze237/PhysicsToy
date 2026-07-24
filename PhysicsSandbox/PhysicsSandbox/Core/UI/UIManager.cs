//Global defines for ui types
global using UIElementID = uint;
global using UILayerID = uint;
using System.Numerics;
using PhysicsSandbox.Core.UI.Toolbar;
using PhysicsSandbox.Utils;
using Raylib_cs;
using static PhysicsSandbox.Core.UI.FontManager;

//TODO: Add a way for the ui manager to flag inputs as claimed for this tick, so that the game window doesn't process them
namespace PhysicsSandbox.Core.UI;
public sealed class UIManager
{
    //Members

    //Singleton
    private static readonly UIManager m_instance = new UIManager();
    public static UIManager Instance => m_instance;

    private readonly Raylib_cs.Color c_toolbarColor = Raylib_cs.Color.DarkGray;
    private readonly int c_toolbarBuffer = 15;
    public readonly int c_toolbarHeight = (int)(0.03 * Program.c_screenHeight); //This should probably live somewhere else

    //Each ui element gets a unique ID
    private UIElementID m_nextID = 0;

    /// Layers are sorted by ID, with higher IDs rendered on top of lower ones
    private SortedDictionary<UILayerID, HashSet<UIElementID>> m_layers = new();
    private Dictionary<UIElementID, UIElement> m_elements = new();
    private Dictionary<UIElementID, UILayerID> m_elementToLayer = new(); 

    public UIToolbar Toolbar { get; private set; }

    

    //Methods

    //Private singleton constructor
    private UIManager() 
    {
        Toolbar = new UIToolbar(new Vector2Int(Program.c_screenWidth, c_toolbarHeight),
        c_toolbarColor,
        c_toolbarBuffer,
        m_nextID++);

        RegisterElement(Toolbar, 0);
    }

    //-------------
    public UIElementID GetNextID()
    {
        return m_nextID++;
    }

    //-------------
    public  UIElementID RegisterElement
    (
        UIElement i_uiElement,
        UILayerID i_layerID = 0
    ) 
    {
        m_elements[i_uiElement.ID] = i_uiElement;
        m_elementToLayer[i_uiElement.ID] = i_layerID;

        if (!m_layers.TryGetValue(i_layerID, out var layerSet))
        {
            layerSet = new HashSet<UIElementID>();
            m_layers[i_layerID] = layerSet;
        }
        layerSet.Add(i_uiElement.ID);
        
        return i_uiElement.ID; 
    }

   

    //-------------   
    public void UnregisterElement
    (
        UIElementID i_id
    )
    {
        m_elements.Remove(i_id);
        m_elementToLayer.Remove(i_id);
        
        UILayerID layerID = m_elementToLayer[i_id];
        m_layers[layerID].Remove(i_id);
    }

    //-------------   
    public void Update
    (
        float i_deltaTime
    )
    {
       foreach (var layer in m_layers.Reverse())
        {
            foreach (var id in layer.Value)
            {
                m_elements[id].Update(i_deltaTime);
            }
        }
    }

    //-------------   
    public void Render()
    {
       foreach (var layer in m_layers)
        {
            foreach (var id in layer.Value)
            {
                m_elements[id].Render();
            }
        }
    }

    //-------------
    public T? GetElementAs<T>
    (
        UIElementID i_id
    ) where T : UIElement
    {
        return m_elements[i_id] as T;
    }

    //TODO make all the below static methods of the appropriate classes

    public UIElementID CreateAndRegisterTextInput
    (
        Action<string> i_onConfirm, 
        Action i_onCancel, 
        Vector2Int i_size, 
        string i_label,
        string i_text = "",
        UILayerID i_layerID = 0
    )
    {
        TextInput textInput = new TextInput(m_nextID++, i_onConfirm, i_onCancel, i_size, i_label, i_text);
        return RegisterElement(textInput, i_layerID);
    }

    //-------------
    public UIElementID CreateAndRegisterCheckbox
    (
        Vector2Int i_position, 
        string i_label,
        Func<bool> i_getter,
        Action<bool> i_setter,
        UILayerID i_layerID = 0
    )
    {
        UICheckbox checkbox = new UICheckbox(i_position, i_label, i_getter, i_setter, m_nextID++);
        return RegisterElement(checkbox, i_layerID);
    }              

    //-------------
    public UIElementID CreateAndRegisterCheckbox_Relative
    (
        Vector2 i_position, 
        string i_label,
        Func<bool> i_getter,
        Action<bool> i_setter,
        UILayerID i_layerID = 0
    )
    {
        Vector2Int position = UIScaler.RelativeToAbsolute(i_position);
        return CreateAndRegisterCheckbox(position, i_label, i_getter, i_setter, i_layerID);
    }              

    //-------------
    public UIElementID CreateAndRegisterCheckbox
    (
        Vector2Int i_position, 
        string i_label,
        Func<bool> i_getter,
        Action<bool> i_setter,
        Color i_selectedColor,
        Color i_unselectedColor,
        Color i_labelColor,
        UILayerID i_layerID = 0
    )
    {
        UICheckbox checkbox = new UICheckbox(i_position, i_label, i_getter, i_setter, i_selectedColor, i_unselectedColor, i_labelColor, m_nextID++);
        return RegisterElement(checkbox, i_layerID);
    }     

    //-------------
    public UIElementID CreateAndRegisterCheckbox_Relative
    (
        Vector2 i_position, 
        string i_label,
        Func<bool> i_getter,
        Action<bool> i_setter,
        Color i_selectedColor,
        Color i_unselectedColor,
        Color i_labelColor,
        UILayerID i_layerID = 0
    )
    {
        Vector2Int position = UIScaler.RelativeToAbsolute(i_position);
        UICheckbox checkbox = new UICheckbox(position, i_label, i_getter, i_setter, i_selectedColor, i_unselectedColor, i_labelColor, m_nextID++);
        return RegisterElement(checkbox, i_layerID);
    }              

    //-------------
    public UIElementID CreateAndRegisterBox
    (
        Vector2Int i_position,
        Vector2Int i_size,
        Color i_color,
        UILayerID i_layerID = 0
    )
    {
        UIBox box = new UIBox(i_position, i_size, i_color, m_nextID++);
        return RegisterElement(box, i_layerID);
    }

    //-------------
    public UIElementID CreateAndRegisterBox_Relative
    (
        Vector2 i_position,
        Vector2 i_size,
        Color i_color,
        UILayerID i_layerID = 0
    )
    {
        Vector2Int position = UIScaler.RelativeToAbsolute(i_position);
        Vector2Int size = UIScaler.RelativeToAbsolute(i_size);
        UIBox box = new UIBox(position, size, i_color, m_nextID++, false /*No Scaling*/);
        return RegisterElement(box, i_layerID);
    }

    //-------------
    public UIElementID CreateAndRegisterRoundedBox
    (
        Vector2Int i_position,
        Vector2Int i_size,
        Color i_color,
        float i_rounding = 0.2f,
        UILayerID i_layerID = 0
    )
    {
        UIBox box = new UIBox(i_position, i_size, i_rounding, i_color, m_nextID++);
        return RegisterElement(box, i_layerID);
    }

    //-------------
    public UIElementID CreateAndRegisterRoundedBox_Relative
    (
        Vector2 i_position,
        Vector2 i_size,
        Color i_color,
        float i_rounding = 0.2f,
        UILayerID i_layerID = 0
    )
    {
        Vector2Int position = UIScaler.RelativeToAbsolute(i_position);
        Vector2Int size = UIScaler.RelativeToAbsolute(i_size);
        UIBox box = new UIBox(position, size, i_rounding, i_color, m_nextID++, false /*No Scaling*/);
        return RegisterElement(box, i_layerID);
    }

    //-------------
    public UIElementID CreateAndRegisterText
    (
        string i_text,
        Color i_color,
        FontStyle i_style,
        Vector2Int i_position,
        int i_fontSize,
        UILayerID i_layerID = 0
    )
    {
        UIText text = new UIText(i_text, i_color, i_style, i_position, i_fontSize, m_nextID++);
        return RegisterElement(text, i_layerID);
    }

    //-------------
    public UIElementID CreateAndRegisterText_Relative
    (
        string i_text,
        Color i_color,
        FontStyle i_style,
        Vector2 i_position,
        int i_fontSize,
        UILayerID i_layerID = 0
    )
    {
        Vector2Int position = UIScaler.RelativeToAbsolute(i_position);
        UIText text = new UIText(i_text, i_color, i_style, position, i_fontSize, m_nextID++, false /*No Scaling*/);
        return RegisterElement(text, i_layerID);
    }

  
}


