using System;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI.Toolbar;

public abstract class ToolbarElement
{  
    protected static readonly bool c_debugDrawBounds = true;

    //-------------
    public Rectangle RenderBounds { get; set; } 
    public int BaseWidth { get; private set; }

    //-------------
    public void SetBaseWidthMult
    (
        float i_mult
    )
    {
        BaseWidth = (int)(Program.c_screenWidth * i_mult);
        UIManager.Instance.Toolbar.UpdateElementBounds();
    }

    //-------------
    public ToolbarElement
    (
        int i_baseWidth
    )
    {
        BaseWidth = i_baseWidth;
    }

    public abstract void Render();
    public virtual void Update(float i_deltaTime) { }
}

