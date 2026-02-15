using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using PhysicsSansbox.TileRender;
using PhysicsSansbox;
using PhysicsSansbox.PathfindTester;

class Program
{
    public static readonly int c_screenWidth = 1000;
    public static readonly int c_screenHeight = 1000;
    public static readonly float c_fixedTimeStep = 1f / 60f;

    static void Main()
    {
        float timeAccumulator = 0f;
        Renderer renderer;
        List<LogicManager> managers;
        WorldBuilder world = new PathfindWorld();
        world.BuildWorld(out renderer, out managers);


        InitWindow(c_screenWidth, c_screenHeight, "Raylib C# Sandbox");
        SetTargetFPS(60);

        while (!WindowShouldClose())
        {
            //Fixed Update
            timeAccumulator += Raylib.GetFrameTime();
            while (timeAccumulator >= c_fixedTimeStep)
            {
                timeAccumulator -= c_fixedTimeStep;
                foreach(LogicManager manager in managers)
                {
                    manager.FixedUpdate();
                }
            }

            //Render
            BeginDrawing();
            ClearBackground(Raylib_cs.Color.Black);
            renderer.Render(Raylib.GetFrameTime());
            EndDrawing();
        }

        CloseWindow();
    }
}
