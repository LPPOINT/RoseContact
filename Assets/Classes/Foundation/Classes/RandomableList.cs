using System.Collections.Generic;
using UnityEngine;

namespace Assets.Classes.Foundation.Classes
{
    public static class RandomableList
    {





        private static Dictionary<string, List<int>> usedIndexesDictionary = new Dictionary<string, List<int>>(); 

        public static TType GetUniqueRandom<TType>(List<TType> list, string uniqueTag)
        {
            if (!usedIndexesDictionary.ContainsKey(uniqueTag))
            {
                usedIndexesDictionary.Add(uniqueTag, new List<int>());
            }

            var usedIndexes = usedIndexesDictionary[uniqueTag];

            

            if (usedIndexes.Count == list.Count) usedIndexes.Clear();

            var index = 0;
            var loopChecker = 0;

            do
            {
                loopChecker++;
                index = Random.Range(0, list.Count);
            } while (usedIndexes.Contains(index) || loopChecker > list.Count * 5);

            usedIndexes.Add(index);

            return list[index];
        }

    }
}
