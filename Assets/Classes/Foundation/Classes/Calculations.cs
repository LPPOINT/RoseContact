using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Classes.Foundation.Classes
{
    public static class Calculations
    {
        public static float GetRotationAngle(Vector2 a, Vector2 b)
        {

            ////http://ru.onlinemschool.com/math/library/vector/angl/

            //var n = b;
            //n.Normalize();
            //var axis = new Vector2(1, 0);
            //var dot = Vector2.Dot(n, axis);

            //var res = dot/(axis.magnitude*n.magnitude);

            //return (res * 180) / (float)Math.PI;

            var n = b - a;
            var ar = (float)(Math.Atan2(n.x, n.y));
            return ar * (180f / (float)Math.PI);

        }

        public static double GetAngleBetweenTwoLines(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {

            var x1 = b1.x;
            var x2 = b2.x;
            var x3 = a1.x;
            var x4 = a2.x;

            var y1 = b1.y;
            var y2 = b2.y;
            var y3 = a1.y;
            var y4 = a2.y;

            var a = Math.Atan2(y2 - y1, x2 - x1) - Math.Atan2(y4 - y3, x4 - x3);

            return a * 180 / Math.PI;
        }

        public static void Test()
        {
            var shouldBe0 = GetRotationAngle(new Vector2(1, 0), new Vector2(2, 0));

            Debug.Log("0 = " + shouldBe0);
        }

    }
}
