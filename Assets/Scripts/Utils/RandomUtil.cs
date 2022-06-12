using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// simple shuffles the given array
    /// </summary>
    public static class RandomUtil
    {
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                // according to unity doc https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Random.Range.html
                // Unity random will behave for integer
                // maxExcusive is exclusive, so for example Random.Range(0, 10) will return a value between 0 and 9,
                int k = Random.Range(0, n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }
    }
}