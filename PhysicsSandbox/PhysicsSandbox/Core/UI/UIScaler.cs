using Raylib_cs;
using System;
using System.Collections.Generic;

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
}