namespace PhysicsSansbox.Core;

public abstract class Renderer
{
    //-------------------
    public void Render
    (
        float i_alpha
    )
    {
        RenderImpl(i_alpha);

        foreach (RenderableObject renderable in m_renderableOjects)
        {
            renderable.Render(i_alpha);
        }
    }

    //-------------------
    public void AddRenderable
    (
        RenderableObject i_renderable
    )
    {
        m_renderableOjects.Add(i_renderable);
    }

    //-------------------
    public abstract void RenderImpl(float i_dt);


    //Members
    private List<RenderableObject> m_renderableOjects = new List<RenderableObject>();
}
