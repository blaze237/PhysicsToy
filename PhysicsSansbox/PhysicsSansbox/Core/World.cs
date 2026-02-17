namespace PhysicsSansbox.Core;

internal abstract class World
{
    //-----------------------
    public void _Init
    (
    )
    {
        m_renderer = CreateRenderer();
        Init();
    }

    //-----------------------   
    public void _FixedUpdate
    (
        float i_fixedDeltaTime
    )
    {
        //World script gets updated first
        FixedUpdate(i_fixedDeltaTime);
        //Then all the managers
        foreach (LogicManager manager in m_managers)
        {
            manager.FixedUpdate(i_fixedDeltaTime);
        }
    }
    //-----------------------
    public void _Update
    (
        float i_deltaTime
    )
    {
        //World script gets updated first
        Update(i_deltaTime);
        //Then all the managers
        foreach (LogicManager manager in m_managers)
        {
            manager.Update(i_deltaTime);
        }
    }

    //-----------------------
    public void Render
    (
        float i_alpha
    )
    {
        m_renderer.Render(i_alpha);
    }

    //-----------------------
    protected void RegisterManager
    (
        LogicManager i_manager
    )
    {
        m_managers.Add(i_manager);
    }

    //-----------------------
    protected void UnregisterManager
    (
        LogicManager i_manager
    )
    {
        m_managers.Remove(i_manager);
    }



    //Interfaces
    public abstract void Init();
    public abstract Renderer CreateRenderer();
    public abstract void Destroy();
    public abstract void FixedUpdate(float i_fixedDeltaTime);
    public abstract void Update(float i_deltaTime);

    //Members
    protected List<LogicManager> m_managers = new List<LogicManager>();
    protected Renderer m_renderer = null!;
}
