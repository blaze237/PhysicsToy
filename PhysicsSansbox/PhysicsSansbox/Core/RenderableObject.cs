namespace PhysicsSansbox.Core;

public abstract class RenderableObject
{
    // Methods
    //-------------------
    protected RenderableObject
    (
        Renderer i_renderer
    )
    {
        i_renderer.AddRenderable(this);
    }


    //-------------------
    public abstract void Render(float i_dt);
}
