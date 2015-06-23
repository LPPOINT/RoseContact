using UnityEngine;

namespace Assets.Classes.Foundation.Extensions
{
    public static class TransformExtensions
    {
        public static Vector3 WorldToLocal(this Transform t, Vector3 position)
        {
            return t.transform.position - position;
        }

        public static Transform SetScaleXY(this Transform t, float scale)
        {
            t.localScale = new Vector3(scale, scale, t.localScale.z);
            return t;
        }
        public static Transform SetScaleX(this Transform t, float scale)
        {
            t.localScale = new Vector3(scale, t.localScale.y, t.localScale.z);
            return t;
        }
        public static Transform SetScaleY(this Transform t, float scale)
        {
            t.localScale = new Vector3(t.localScale.x, scale, t.localScale.z);
            return t;
        }

        public static float GetScaleX(this Transform t)
        {
            return t.localScale.x;
        }
        public static float GetScaleY(this Transform t)
        {
            return t.localScale.y;
        }

        public static Transform SetPositionXY(this Transform t, Vector2 xy)
        {
            return t.SetPositionXY(xy.x, xy.y);
        }
        public static Transform SetPositionXY(this Transform t, float newX, float newY)
        {
            return t.SetPositionX(newX).SetPositionY(newY);
        }

        public static Transform SetPositionXY(this Transform t, Vector3 other)
        {
            return SetPositionXY(t, other.x, other.y);
        }
        public static Transform SetPositionX(this Transform t, float newX)
        {
            t.position = new Vector3(newX, t.position.y, t.position.z);
            return t;
        }
        public static Transform SetPositionY(this Transform t, float newY)
        {
            t.position = new Vector3(t.position.x, newY, t.position.z);
            return t;
        }
        public static Transform SetPositionZ(this Transform t, float newZ)
        {
            t.position = new Vector3(t.position.x, t.position.y, newZ);
            return t;
        }

        public static float GetPositionX(this Transform t)
        {
            return t.position.x;
        }
        public static float GetPositionY(this Transform t)
        {
            return t.position.y;
        }
        public static float GetPositionZ(this Transform t)
        {
            return t.position.z;
        }

        public static Transform SetRotationX(this Transform t, float x)
        {
            t.rotation = Quaternion.Euler(x, t.rotation.eulerAngles.y, t.rotation.eulerAngles.z);
            return t;
        }
        public static Transform SetRotationY(this Transform t, float y)
        {
            t.rotation = Quaternion.Euler(t.rotation.eulerAngles.x, y, t.rotation.eulerAngles.z);
            return t;
        }
        public static Transform SetRotationXY(this Transform t, float x, float y)
        {
            t.rotation = Quaternion.Euler(x, y, t.rotation.eulerAngles.z);
            return t;
        }

        public static Transform SetRotationZ(this Transform t, float z)
        {
            t.rotation = Quaternion.Euler(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, z);
            return t;
        }

        public static float GetRotationX(this Transform t)
        {
            return t.rotation.eulerAngles.x;
        }
        public static float GetRotationY(this Transform t)
        {
            return t.rotation.eulerAngles.y;
        }

        public static float GetRotationZ(this Transform t)
        {
            return t.rotation.eulerAngles.z; 
        }

    }
}