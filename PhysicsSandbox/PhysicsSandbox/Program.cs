using PhysicsSandbox.Core;
using PhysicsSandbox.Core.UI;
using PhysicsSandbox.PathfindTester;
using PhysicsSandbox.Utils;
using static Raylib_cs.Raylib;

class Program
{
    public static readonly int c_screenWidth = 1000;
    public static readonly int c_screenHeight = 1000;
    public static readonly float c_fixedTimeStep = 1f / 60f;

    static void Main()
    {
        float timeAccumulator = 0f;
        World world = new PathfindWorld();
        UIManager uiManager = UIManager.Instance;

        world._Init();

        InitWindow(c_screenWidth, c_screenHeight, "Raylib C# Sandbox");

        //Debug only
        // bool checkboxValue = false;
        // uiManager.CreateAndRegisterRoundedBox(new Vector2Int(25, 25), new Vector2Int(150, 100), new Raylib_cs.Color(0, 0, 0, 128));
        // uiManager.CreateAndRegisterCheckbox(new Vector2Int(50, 50), "Test", () => checkboxValue, (value) => checkboxValue = value);

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
            uiManager.Update(frameTime); //UI is updated first so that it can claim inputs (once implemented)
            world._Update(frameTime);

            //Render
            BeginDrawing();
            ClearBackground(Raylib_cs.Color.White); //Should the renderer handle this
            world.Render(alpha);
            uiManager.Render(); //UI is always rendered on top
            EndDrawing();
        }

        CloseWindow();
    }
}
