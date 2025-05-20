using System.Collections.Generic;
using System;

public static class CommonUtility
{
    public static void Shuffle<T>(List<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static void ShuffleTogether<T, U>(List<T> list1, List<U> list2)
    {
        Random rng = new Random();
        int n = list1.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);

            // Swap elements in both lists
            (list1[i], list1[j]) = (list1[j], list1[i]);
            (list2[i], list2[j]) = (list2[j], list2[i]);
        }
    }
}
