using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Classes.Foundation.Classes
{
    [Serializable]
    public class Range
    {

        public Range()
        {
            
        }

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min;
        public float Max;

        public float GenerateRandom()
        {
            return Random.Range(Min, Max);
        }



    }

     [Serializable]
    public class NumericRange
    {
        public NumericRange()
        {
            
        }

        public NumericRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Min;
        public int Max;

        public int GenerateRandom()
        {
            return Random.Range(Min, Max);
        }

    }

     [Serializable]
     public class VectorRange
     {
         public VectorRange()
         {

         }

         public VectorRange(Vector3 min, Vector3 max)
         {
             Min = min;
             Max = max;
         }

         public Vector3 Min;
         public Vector3 Max;

         public Vector3 GenerateRandom()
         {
             return new Vector3(Random.Range(Min.x, Max.x), Random.Range(Min.y, Max.y), Random.Range(Min.z, Max.z));
         }

     }

}
