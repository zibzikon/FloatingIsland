using System;
using System.Collections;
using System.Collections.Generic;

namespace Extentions
{
    public static class NeighboursExtention
    {
        public static IEnumerable<T> ToEnumerable<T>(this Neighbors3<T> neighbors)
        {
            var neighboursList = new List<T>();
            for (int i = 0; i < Neighbors3<T>.Length; i++)
            {
                if (neighbors[i] != null)
                {
                    neighboursList.Add(neighbors[i]);
                }
            }

            return neighboursList;
        }
        
        public static IEnumerable<T> ToEnumerable<T>(this Neighbors2<T> neighbors)
        {
            var neighboursList = new List<T>();
            for (int i = 0; i < Neighbors2<T>.Length; i++)
            {
                if (neighbors[i] != null)
                {
                    neighboursList.Add(neighbors[i]);
                }
            }

            return neighboursList;
        }

        public static Neighbors2<T> ToNeighbors2<T>(this Neighbors3<T> neighbors)
        {
            return new Neighbors2<T>()
                { Right = neighbors.Right, Left = neighbors.Left, Back = neighbors.Back, Foward = neighbors.Foward };
        }
    }
}