using System;
using UnityEngine;

namespace Assets.Classes.Foundation.Extensions
{
    public static class RectTransformExtensions
    {
        public static float GetWorldWidth(this RectTransform t)
        {
            //var corners = new Vector3[4];
            //Debug.Log("Rect " + t.rect);
            //t.GetWorldCorners(corners);
            //for (var i = 0; i < 4; i++)
            //{
            //    Debug.Log("[" + i + "] = " + corners[i]);
            //}
            //var w = Math.Abs(corners[1].x - corners[2].x);
            //return w;
            var min = t.rect.xMin;
            var max = t.rect.xMax;
            var w1 = t.TransformPoint(new Vector3(t.rect.xMin, t.rect.yMin));
            var w2 = t.TransformPoint(new Vector3(t.rect.xMax, t.rect.yMin));
            var w = Math.Abs(w1.x - w2.x);
            return w;
        }

        public static float GetWorldHeight(this RectTransform transform)
        {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            var h = Math.Abs(corners[0].y - corners[1].y);
            return h;
        }

        public static Rect GetWorldRect(this RectTransform transform)
        {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            return new Rect(corners[1].x, corners[1].y, Math.Abs(corners[2].x - corners[1].x), Math.Abs(corners[0].y - corners[1].y));
        }
    }
}
