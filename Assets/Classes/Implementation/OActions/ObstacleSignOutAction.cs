using Assets.Classes.Foundation.Extensions;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation.OActions
{
    public class ObstacleSignOutAction : ObstacleAction
    {

        public enum SignOutSide
        {
            DetectAutomatically,
            Left,
            Right
        }
        public enum SignOutMode
        {
            Show,
            Hide
        }

        public SignOutSide Side;
        public SignOutMode Mode = SignOutMode.Show;
        public Ease Ease = Ease.OutBack;
        public float Time = 0.3f;
        public bool StartHided = true;

        private Vector3 targetPosition;
        private Vector3 startPosition;

        public SignOutSide AutoDetectSide()
        {
            if(transform.position.x < GameCamera.Instance.Viewport.xMin + GameCamera.Instance.Viewport.width / 2) return SignOutSide.Left;
            return SignOutSide.Right;
        }

        public void Hide()
        {
            transform.position = startPosition;
        }

        public void Show()
        {
            transform.position = targetPosition;
        }

        public override void Play()
        {
            if (Mode == SignOutMode.Show)
            {
                Hide();

                transform.DOMove(targetPosition, Time)
                    .SetEase(Ease)
                    .OnComplete(NotifyActionComplete);
            }
            else if (Mode == SignOutMode.Hide)
            {
                Show();
                transform.DOMove(startPosition, Time)
                    .SetEase(Ease)
                    .OnComplete(() =>
                                {
                                    NotifyActionComplete();
                                    gameObject.SetActive(false);
                                });
            }
            base.Play();
        }

        private void Start()
        {
            targetPosition = transform.position;

            if (Side == SignOutSide.DetectAutomatically)
                Side = AutoDetectSide();

            if (Side == SignOutSide.Left)
            {
                startPosition = new Vector3(GameCamera.Instance.Viewport.xMin - renderer.bounds.size.x / 2, transform.position.y, transform.position.z);
            }
            else if (Side == SignOutSide.Right)
            {
                startPosition = new Vector3(GameCamera.Instance.Viewport.xMax + renderer.bounds.size.x / 2, transform.position.y, transform.position.z);
            }


            if(StartHided)
                Hide();
        }
    }
}
