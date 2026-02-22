using System;

namespace PhysicsSandbox.Core.UI;

public abstract class UIElement
{
    public abstract void Render();
    public virtual void Update(float i_deltaTime) {}
}
