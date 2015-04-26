using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Foundation.Enums;
using Assets.Classes.Foundation.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Implementation
{
    public class Borders : SingletonEntity<Borders>
    {



        #region Colliders

        public bool ShouldGenerateColliders = true;

        public float LeftColliderMargin = 0;
        public float RightColliderMargin = 0;
        public float TopColliderMargin = 0;
        public float BottomColliderMargin = 0;

        public BoxCollider2D LeftCollider { get; private set; }
        public BoxCollider2D RightCollider { get; private set; }
        public BoxCollider2D TopCollider { get; private set; }
        public BoxCollider2D BottomCollider { get; private set; }

        public BoxCollider2D GetBorderCollider(Direction colliderSide)
        {
            switch (colliderSide)
            {
                case Direction.Left:
                    return LeftCollider;
                case Direction.Right:
                    return RightCollider;
                case Direction.Top:
                    return TopCollider;
                case Direction.Bottom:
                    return BottomCollider;
                default:
                    return null;
            }
        }
        public bool IsBordersCollider(BoxCollider2D c)
        {
            if (c == null)
                return false;
            return c.Equals(LeftCollider) || c.Equals(RightCollider) || c.Equals(TopCollider) ||
                   c.Equals(BottomCollider);
        }

        private BoxCollider2D CreateCollider(Rect box, string goName)
        {
            var go = new GameObject(goName);
            go.transform.position = new Vector3(box.xMin, box.yMax);
            go.transform.parent = transform;
            var c = go.AddComponent<BoxCollider2D>();
            c.size = new Vector2(box.width, box.height);
            c.offset = new Vector2(box.width / 2, -box.height / 2);
            return c;
        }

        public void GenerateColliders()
        {
            GameCamera.Instance.InvalidateViewport();
            var h = GameCamera.Instance.Viewport.height;
            var w = GameCamera.Instance.Viewport.width;

            var lb = GameCamera.Instance.Camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
            var lt = GameCamera.Instance.Camera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
            var rt = GameCamera.Instance.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            var size = 1.488f;

            LeftCollider =
                CreateCollider(
                    new Rect(lb.x - size - LeftColliderMargin, lb.y, size, h), "Left");
            TopCollider =
                CreateCollider(
                    new Rect(lt.x, lt.y + TopColliderMargin,
                        w,
                        size), "Top");
            RightCollider =
                CreateCollider(
                    new Rect(rt.x + RightColliderMargin, lb.y,
                        size, h), "Right");
            BottomCollider =
                CreateCollider(
                    new Rect(lb.x, lb.y - size - BottomColliderMargin, w,
                        size), "Bottom");

        }

        #endregion


        #region Unity callbacks

        protected override void Awake() 
        {
            transform.parent = GameCamera.Instance.transform;

            if (ShouldGenerateColliders)
            {
                GenerateColliders();
            }
        }

        #endregion
    }
}
