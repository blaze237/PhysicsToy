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
        World world = new PathfindWorld();
        world._Init();


        InitWindow(c_screenWidth, c_screenHeight, "Raylib C# Sandbox");
        SetTargetFPS(60);

        while (!WindowShouldClose())
        {
            float frameTime = GetFrameTime();
            // Cap frame time to avoid spiral of death
            frameTime = MathF.Min(frameTime, 0.25f);

            //Fixed Update
            timeAccumulator += frameTime;
            while (timeAccumulator >= c_fixedTimeStep)
            {
                timeAccumulator -= c_fixedTimeStep;
                world._FixedUpdate(c_fixedTimeStep); 
            }
            float alpha = (float)(timeAccumulator / c_fixedTimeStep);


            //Variable Update
            world._Update(frameTime);


            //Render
            BeginDrawing();
            ClearBackground(Raylib_cs.Color.Black);
            world.Render(alpha);
            EndDrawing();
        }

        CloseWindow();
    }
}
