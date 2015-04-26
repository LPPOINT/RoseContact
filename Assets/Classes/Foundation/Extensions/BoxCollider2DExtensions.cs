using System;
using UnityEngine;

namespace Assets.Classes.Foundation.Extensions
{
    public static class BoxCollider2DExtensions
    {
        public static Rect GetWorldRect(this BoxCollider2D boxCollider2D)
        {
            float worldRight = boxCollider2D.transform.TransformPoint(boxCollider2D.offset + new Vector2(boxCollider2D.size.x * 0.5f, 0)).x;
            float worldLeft = boxCollider2D.transform.TransformPoint(boxCollider2D.offset - new Vector2(boxCollider2D.size.x * 0.5f, 0)).x;

            float worldTop = boxCollider2D.transform.TransformPoint(boxCollider2D.offset + new Vector2(0, boxCollider2D.size.y * 0.5f)).y;
            float worldBottom = boxCollider2D.transform.TransformPoint(boxCollider2D.offset - new Vector2(0, boxCollider2D.size.y * 0.5f)).y;

            return new Rect(
                worldLeft,
                worldBottom,
                Math.Abs(worldRight - worldLeft),
                Math.Abs(worldTop - worldBottom)
                );
        }
        public static Rect GetWorldRect(this BoxCollider2D boxCollider2D, float mul)
        {
            float worldRight = boxCollider2D.transform.TransformPoint(boxCollider2D.offset + new Vector2(boxCollider2D.size.x * 0.5f * mul, 0)).x;
            float worldLeft = boxCollider2D.transform.TransformPoint(boxCollider2D.offset - new Vector2(boxCollider2D.size.x * 0.5f * mul, 0)).x;

            float worldTop = boxCollider2D.transform.TransformPoint(boxCollider2D.offset + new Vector2(0, boxCollider2D.size.y * 0.5f * mul)).y;
            float worldBottom = boxCollider2D.transform.TransformPoint(boxCollider2D.offset - new Vector2(0, boxCollider2D.size.y * 0.5f * mul)).y;

            return new Rect(
                worldLeft,
                worldBottom,
                Math.Abs(worldRight - worldLeft),
                Math.Abs(worldTop - worldBottom)
                );
        }
    }
}
