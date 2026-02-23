//Global defines for ui types
global using UIElementID = uint;
global using UILayerID = uint;

using System;
using System.Numerics;
using PhysicsSandbox.Utils;
using Raylib_cs;
using static PhysicsSandbox.Core.UI.UIText;


namespace PhysicsSandbox.Core.UI;
public sealed class UIManager
{
    //Members

    //Singleton
    private static readonly UIManager m_instance = new UIManager();
    public static UIManager Instance => m_instance;

    //Each ui element gets a unique ID
    private UIElementID m_nextID = 0;

    /// Layers are sorted by ID, with higher IDs rendered on top of lower ones
    private SortedDictionary<UILayerID, HashSet<UIElementID>> m_layers = new();
    private Dictionary<UIElementID, UIElement> m_elements = new();
    private Dictionary<UIElementID, UILayerID> m_elementToLayer = new(); 


    //TODO: Add a way for the ui manager to flag inputs as claimed for this tick, so that the game window doesn't process them


    //Methods
    //-------------
    private UIElementID RegisterElement
    (
        UIElement i_uiElement,
        UILayerID i_layerID = 0
    ) 
    {
        m_elements[m_nextID] = i_uiElement;
        m_elementToLayer[m_nextID] = i_layerID;

        if (!m_layers.TryGetValue(i_layerID, out var layerSet))
        {
            layerSet = new HashSet<UIElementID>();
            m_layers[i_layerID] = layerSet;
        }
        layerSet.Add(m_nextID);
        
        return m_nextID++; 
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
    public void Render
    (

    )
    {
       foreach (var layer in m_layers)
        {
            foreach (var id in layer.Value)
            {
                m_elements[id].Render();
            }
        }
    }

    //TODO take in screen space coords not raw pixel positions

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
        UICheckbox checkbox = new UICheckbox(i_position, i_label, i_getter, i_setter);
        return RegisterElement(checkbox, i_layerID);
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
        UICheckbox checkbox = new UICheckbox(i_position, i_label, i_getter, i_setter, i_selectedColor, i_unselectedColor, i_labelColor);
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
        UIBox box = new UIBox(i_position, i_size, i_color);
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
        UIBox box = new UIBox(i_position, i_size, i_rounding, i_color);
        return RegisterElement(box, i_layerID);
    }

    //-------------
    public UIElementID CreateAndRegisterText
    (
        string i_text,
        Color i_color,
        FontStyle i_style,
        Vector2 i_position,
        int i_fontSize,
        UILayerID i_layerID = 0
    )
    {
        UIText text = new UIText(i_text, i_color, i_style, i_position, i_fontSize);
        return RegisterElement(text, i_layerID);
    }

    //Private singleton constructor
    private UIManager() {}

  
}


