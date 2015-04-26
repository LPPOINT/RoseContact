using Assets.Classes.Foundation.Enums;
using Assets.Classes.Foundation.Extensions;
using UnityEngine;

namespace Assets.Classes.Core
{
    public class GameCameraBase : SingletonEntity<GameCameraBase>
    {
        public Camera Camera;


        #region Viewport

        private Rect lastViewport;
        private bool isViewportChanged;

        public Rect Viewport
        {
            get
            {
                if(isViewportChanged) InvalidateViewport();
                return lastViewport;
            }
        }

        public void InvalidateViewport()
        {
            lastViewport = Camera.GetCameraViewRect();
            isViewportChanged = false;
        }

        #endregion

        #region Collision

        public enum ViewportContainmentType
        {
            Inside,
            Outside
        }

        private bool IsPointIsInsideOfHorizontalViewport(float point, float border, HorizontalDirection d)
        {
            if (d == HorizontalDirection.Left) return point <= border;
            return point >= border;
        }
        private bool IsPointIsInsideOfVerticalViewport(float point, float border, VerticalDirection d)
        {
            if (d == VerticalDirection.Top) return point <= border;
            return point >= border;
        }

        public ViewportContainmentType DetectContainmentType(Rect objWorldRect)
        {
            if(objWorldRect.ContainsOrIntersects(Viewport)) return ViewportContainmentType.Inside;
            return ViewportContainmentType.Outside;
        }

        #endregion

        #region Camera properties tracking
        public Vector3 LastPosition { get; private set; }
        public float LastOrthographicSize { get; private set; }

        #endregion

        #region Utils

        public void SetBackgroundColor(Color c)
        {
            Camera.backgroundColor = c;
        }

        #endregion

        protected virtual void LateUpdate()
        {
            if (LastPosition != transform.position)
            {
                LastPosition = transform.position;
                isViewportChanged = true;
            }
            if (LastOrthographicSize != Camera.orthographicSize)
            {
                LastOrthographicSize = Camera.orthographicSize;
                isViewportChanged = true;
            }
        }

        protected override void Awake()
        {
            if (Camera == null)
            {
                Camera = Camera.main;
                Debug.LogWarning("GameCamera: CameraToObserve not initialized. Using Camera.main");
            }
            LastPosition = transform.position;
            LastOrthographicSize = Camera.orthographicSize;
            InvalidateViewport();
        }
    }

    public abstract class GameCameraImplementationBase<T> : GameCameraBase where T : GameCameraBase
    {
        private static T implementationInstance;

        public static T ImplementationInstance
        {
            get { return implementationInstance ?? (implementationInstance = Instance as T); }
        }
    }

}
