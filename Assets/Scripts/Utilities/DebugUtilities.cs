using UnityEngine;

public class DebugUtilities
{
    public static bool Verify(bool condition, string message)
    {
        Debug.Assert(condition, message);

        return condition;
    }
}