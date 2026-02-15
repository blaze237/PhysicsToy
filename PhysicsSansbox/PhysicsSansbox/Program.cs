using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using PhysicsSansbox.TileRender;

class Program
{
    const int c_screenWidth = 1000;
    const int c_screenHeight = 1000;



    static void Main()
    {
        TileRenderer m_tileRenderer = new TileRenderer(200, c_screenWidth, c_screenHeight);


        InitWindow(c_screenWidth, c_screenHeight, "Raylib C# Sandbox");
        SetTargetFPS(60);

        while (!WindowShouldClose())
        {
            BeginDrawing();
            ClearBackground(Raylib_cs.Color.Black);

            m_tileRenderer.Render(Raylib.GetFrameTime());


            EndDrawing();
        }

        CloseWindow();
    }
}
