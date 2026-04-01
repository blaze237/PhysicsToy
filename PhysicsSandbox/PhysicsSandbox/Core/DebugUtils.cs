using System.Diagnostics;

namespace PhysicsSandbox.Core;

public static class DebugUtils
{
    //-------------------------------
    [Conditional("DEBUG")]
    public static void Assert
    (
        bool condition, 
        string message
    )
    {
        if (!condition) 
        {
            Console.WriteLine($"Assertion failed: {message}");
            System.Diagnostics.Debugger.Break();
        }
    }

     //-------------------------------
    [Conditional("DEBUG")]
    public static void Assert
    (
        bool condition
    )
    {
        if (!condition) 
        {
            System.Diagnostics.Debugger.Break();
        }
    }
}