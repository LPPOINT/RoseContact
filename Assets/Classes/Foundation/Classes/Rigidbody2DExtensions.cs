using UnityEngine;

namespace Assets.Classes.Foundation.Classes
{
    public static class Rigidbody2DExtensions
    {
        public static void RotateAround(this Rigidbody2D rb, Vector3 center, Vector3 axis, float angle)
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis);
            rb.MovePosition(q * (rb.transform.position - center) + center);
            rb.MoveRotation((rb.transform.rotation * q).eulerAngles.z);
           
        }
    }
}
