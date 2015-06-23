using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation.OActions
{
    public class ObstacleScaleOutAction : ObstacleAction
    {

        public float InitialScale = 0;
        public float Time = 0.3f;
        public Ease Ease = Ease.OutBack;

        private Vector3 end;

        protected override void Awake()
        {
            end = transform.localScale;

            transform.localScale = new Vector3(InitialScale, InitialScale, transform.localScale.z);
            base.Awake();
        }

        public override void Play()
        {
            transform.DOScale(end, Time)
                .SetEase(Ease);
            base.Play();
        }
    }
}
