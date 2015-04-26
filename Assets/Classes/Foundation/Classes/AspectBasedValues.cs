using System;
using UnityEngine;

namespace Assets.Classes.Foundation.Classes
{

    public enum Aspect
    {
        Aspect9x16,
        Aspect3x4,
        Aspect2x3,
        Undefined
    }


    [Serializable]
    public class AspectBasedValues<T>
    {


        public static Aspect DetectAspect(float w, float h)
        {

            const float tolerance = 0.01f;
            if (Math.Abs(w / h - 9f / 16f) <= tolerance) return Aspect.Aspect9x16;
            if (Math.Abs(w / h - 3f / 4f) <= tolerance) return Aspect.Aspect3x4;
            if (Math.Abs(w / h - 2f / 3f) <= tolerance) return Aspect.Aspect2x3;

            return Aspect.Undefined;
        }

        public static Aspect DetectAspect()
        {
            return DetectAspect(Screen.width, Screen.height);
        }


        public T ValueFor9x16;
        public T ValueFor3x4;
        public T ValueFor2x3;


        public T GetValueForAspect(Aspect aspect)
        {
            if (aspect == Aspect.Aspect2x3) return ValueFor2x3;
            if (aspect == Aspect.Aspect3x4) return ValueFor3x4;
            if (aspect == Aspect.Aspect9x16) return ValueFor9x16;

            return default(T);
        }

        public T GetValueForCurrentAspect()
        {
            return GetValueForAspect(DetectAspect());
        }


    }

    [Serializable]
    public class AspectBasedFloatValues : AspectBasedValues<float>
    {
        
    }
    [Serializable]
    public class AspectBasedVector2Values : AspectBasedValues<Vector2>
    {

    }

     [Serializable]
    public class AspectBasedRangeValues : AspectBasedValues<Range>
    {
        
    }

     [Serializable]
     public class AspectBasedTransformValues : AspectBasedValues<Transform>
     {

     }
}
