using PhysicsSandbox.Core;
using PhysicsSandbox.Utils;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

public static class UIScaler
{
    //Members
    public static float RefWidth = 1000f;
    public static float RefHeight = 1000f;
    public static float Scale
    {
        get
        {
            float scaleX = (float)Raylib.GetScreenWidth() / RefWidth;
            float scaleY = (float)Raylib.GetScreenHeight() / RefHeight;
            return MathF.Min(scaleX, scaleY);
        }
    }

    //Methods

    //-------------
    public static Vector2Int RelativeToAbsolute
    (
        Vector2 relative
    )
    {
        DebugUtils.Assert(relative.X >= 0 && relative.X <= 1);
        DebugUtils.Assert(relative.Y >= 0 && relative.Y <= 1);
        float screenWidth = Raylib.GetScreenWidth();
        float screenHeight = Raylib.GetScreenHeight();
        return new Vector2Int((int)(relative.X * screenWidth), (int)(relative.Y * screenHeight));
    }

    //--------------
    public static void ScaleRect
    (
        ref Rectangle o_rect
    )
    {
        o_rect.X  *= Scale;
        o_rect.Y *= Scale;
        o_rect.Width *= Scale;
        o_rect.Height *= Scale;
    }

    //--------------
    public static float ScaleValue
    (
        float value
    )
    {
        return value * Scale;
    }

    //--------------
    public static int ScaleValue
    (
        int value
    )
    {
        return (int)(value * Scale);
    }

    //--------------
    public static Vector2 ScaleValue
    (
        Vector2 value
    )
    {
        return new Vector2(value.X * Scale, value.Y * Scale);
    }
}
