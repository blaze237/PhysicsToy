using System;

namespace PhysicsSandbox.Core.UI;

public abstract class UIElement
{
    public readonly UIElementID ID;
    
    public UIElement(UIElementID i_id)
    {
        ID = i_id;
    }
    
    public abstract void Render();
    public virtual void Update(float i_deltaTime) {}
}
