using System;
using Raylib_cs;

namespace PhysicsSandbox.Core.UI.Toolbar;

public abstract class ToolbarElement : UIElement
{  
    public Rectangle RenderBounds { get; set; } 
    public int BaseWidth { get; }

    public ToolbarElement
    (
        int i_baseWidth,
        UIElementID i_id
    )
    : base(i_id)
    {
        BaseWidth = i_baseWidth;
    }
}

