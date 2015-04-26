using UnityEngine;

namespace Assets.Classes.Foundation.Extensions
{
    public static class GameObjectExtensions
    {
        // This checks the intersection between two rectangles.
        public static bool Intersect(Rect a, Rect b)
        {
            // Basic AABB collision detection. All of these must be true for there to be a collision.
            bool comp1 = a.yMin > b.yMax;
            bool comp2 = a.yMax < b.yMin;
            bool comp3 = a.xMin < b.xMax;
            bool comp4 = a.xMax > b.xMin;

            // This will only return true if all are true.
            return comp1 && comp2 && comp3 && comp4;
        }


        public static Rect FindBoundsRectangle(this GameObject a)
        {
            // Finding the BoxCollider2D for the GameObject.
            BoxCollider2D aCollider = a.GetComponent<BoxCollider2D>();

            // Grabbing the GameObject position, converting it to Vector2.
            Vector2 aPos = a.transform.position.ToVector2();

            // Now that we have the object's worldspace location, we offset that to the BoxCollider's center
            aPos += aCollider.offset;
            // From the center, we find the top/left corner by cutting the total height/width in half and
            // offset by that much
            aPos.x -= aCollider.size.x / 2;
            aPos.y += aCollider.size.y / 2;

            // Create a rectangle based on the top/left corner, and extending the rectangle
            // to the width/height
            return new Rect(aPos.x, aPos.y, aCollider.size.x, -aCollider.size.y);
        }

        public static bool IsIntersects(this GameObject a, GameObject b)
        {
            Rect aBox = a.FindBoundsRectangle();
            Rect bBox = b.FindBoundsRectangle();

            // Find out if these guys intersect
            return Intersect(aBox, bBox);
        }
    }
}
