using System;
using PhysicsSandbox.Utils;
using Raylib_cs;

namespace PhysicsSandbox.Core;

using UIElementID = uint;
using UILayerID = uint;


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

    //Todo grab the screen size from the game window and use it to set the scale for all elements

    //Potential todo. Add a way for the ui manager to flag inputs as claimed for this tick, so that the game window doesn't process them


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
       foreach (var layer in m_layers.Reverse())
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
        float i_scale = 1.0f,
        UILayerID i_layerID = 0
    )
    {
        UICheckbox checkbox = new UICheckbox(i_position, i_label, i_getter, i_setter, i_scale);
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
        float i_scale = 1.0f,
        UILayerID i_layerID = 0
    )
    {
        UICheckbox checkbox = new UICheckbox(i_position, i_label, i_getter, i_setter, i_selectedColor, i_unselectedColor, i_labelColor, i_scale);
        return RegisterElement(checkbox, i_layerID);
    }     

    //Private singleton constructor
    private UIManager() {}

  
}


