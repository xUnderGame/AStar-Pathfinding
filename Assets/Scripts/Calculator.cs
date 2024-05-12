using UnityEngine;

public static class Calculator
{
    public static float distance = 0.523f;
    public static int length = 8;

    // Gets a position on the matrix
    public static Vector3 GetPositionFromMatrix(int[]point)
    {
        return new Vector3(-(length - 1f) * distance + point[1] * 2f * distance,
            (length - 1f) * distance - point[0] * 2f * distance, 0);
    }

    // Checks and returns the distance between two objects
    public static float CheckDistanceToObj(int[] point, int[] obj)
    {
        return Vector3.Distance(GetPositionFromMatrix(point), GetPositionFromMatrix(obj));
    }
}
