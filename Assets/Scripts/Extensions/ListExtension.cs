using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Extensions
{
    public static class ListExtension
    {
        public static List<T> Randomize<T>(this List<T> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var tempId = Random.Range(0, list.Count);
                var tempElement = list[tempId];

                list[tempId] = list[i];
                list[i] = tempElement;
            }

            return list;
        }
    }
}