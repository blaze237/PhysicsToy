using System;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI.Toolbar;

public abstract class ToolbarElement
{  
    protected static readonly bool c_debugDrawBounds = true;

    //-------------
    public Rectangle RenderBounds { get; set; } 
    public int BaseWidth { get; private set; }
    private bool m_enabled = true;
    public bool Enabled 
    { 
        get { return m_enabled; }
        set 
        {
            m_enabled = value;
            UIManager.Instance.Toolbar.UpdateElementBounds();
        } 
    }

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

    //-------------
    public virtual void Render() 
    {
        if (c_debugDrawBounds)
        {
            Raylib.DrawRectangleLines((int)RenderBounds.X, (int)RenderBounds.Y, (int)RenderBounds.Width, (int)RenderBounds.Height, Color.Magenta);
        }
    }

    public virtual void Update(float i_deltaTime) { }
}

