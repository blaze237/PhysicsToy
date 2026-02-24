using System.Collections.Generic;
using PhysicsSandbox.Core.UI;


namespace PhysicsToy.Core.UI;

//A basic RAII style wrapper around a bunch of ui elements
public abstract class WorldUIManager
{
    //Implementing classes should use this to register elements and store their ids in m_elements
    public abstract void Init();

    public void Destroy()
    {
        foreach (var element in m_elements)
        {
            UIManager.Instance.UnregisterElement(element);
        }
        m_elements.Clear();
    }

    protected List<UIElementID> m_elements = new List<UIElementID>();
}
