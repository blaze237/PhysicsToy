using System;

namespace PhysicsSandbox.Core;

public abstract class UIElement
{
    public abstract void Render();
    public virtual void Update(float i_deltaTime) {}
}
