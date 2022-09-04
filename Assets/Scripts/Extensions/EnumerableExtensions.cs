using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extentions
{
    public static class EnumerableExtensions
    {

        public static void DestroyObjects(this IEnumerable<MonoBehaviour> monoBehaviours)
        {
            foreach (var behaviour in monoBehaviours)
            {
                if (behaviour != null && behaviour.gameObject != null )
                {          
                    UnityEngine.Object.Destroy(behaviour.gameObject);
                }
            }
        }
    
        public static bool Contains<T>(this IEnumerable<T> enumerable, IEnumerable<T> candidateEnumerable)
        {
            var array = enumerable.ToArray();
            var candidate = candidateEnumerable.ToArray();
            if (IsEmptyLocate(array, candidate))
                return false;

            if (candidate.Length > array.Length)
                return false;

            for (var a = 0; a <= array.Length - candidate.Length; a++)
            {
                if (!array[a].Equals(candidate[0])) continue;
                var i = 0;

                for (; i < candidate.Length; i++)
                {
                    if (false == array[a + i].Equals(candidate[i]))
                        break;
                }
                if (i == candidate.Length)
                    return true;
            }
            return false;
        
        
        }
        private static bool IsEmptyLocate<T>(T[] array, T[] candidate)
        {
            return array == null
                   || candidate == null
                   || array.Length == 0
                   || candidate.Length == 0
                   || candidate.Length > array.Length;
        }
    }
}

