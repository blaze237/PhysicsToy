namespace PhysicsSansbox.Core;

internal abstract class RenderableObject
{
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
