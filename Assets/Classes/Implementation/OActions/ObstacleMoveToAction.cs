using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation.OActions
{
    public class ObstacleMoveToAction : ObstacleAction
    {

        public Transform MoveToTarget;
        public float TimeOrSpeed = 1;
        public bool IsSpeedBased = false;
        public Ease Ease = Ease.Linear;
        public LoopType LoopType = LoopType.Yoyo;
        public int Loops = -1;

        public override void Play()
        {
            transform.DOLocalMove(MoveToTarget.localPosition, TimeOrSpeed)
                .SetEase(Ease)
                .SetLoops(Loops, LoopType)
                .SetSpeedBased(IsSpeedBased);
            base.Play();
        }

        private void OnDrawGizmos()
        {
            if(MoveToTarget == null)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, MoveToTarget.position);
        }

    }
}
