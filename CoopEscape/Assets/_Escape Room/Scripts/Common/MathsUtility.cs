using UnityEngine;

public static class MathsUtility 
{
    public static int Modulo(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
