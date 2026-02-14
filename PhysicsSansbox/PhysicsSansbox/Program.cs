using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

class Program
{
    static void Main()
    {
        const int screenWidth = 800;
        const int screenHeight = 600;

        InitWindow(screenWidth, screenHeight, "Raylib C# Sandbox");
        SetTargetFPS(60);

        while (!WindowShouldClose())
        {
            BeginDrawing();
            ClearBackground(Raylib_cs.Color.Black);

            DrawText("It works!", 320, 280, 20, Raylib_cs.Color.RayWhite);

            EndDrawing();
        }

        CloseWindow();
    }
}
