using Assets.Classes.Foundation.Extensions;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation.OActions
{
    public class ObstacleScaleAction : ObstacleAction
    {
        public float To;
        public float Time;
        public Ease Ease;
        public int Loops = -1;
        public LoopType LoopType = LoopType.Yoyo;

        public bool IsRelative;

        public override void Play()
        {
            var end = IsRelative
                ? new Vector3(transform.GetScaleX()*To, transform.GetScaleY()*To, transform.localScale.z)
                : new Vector3(To, To, transform.localScale.z);

            transform.DOScale(end, Time)
                .SetEase(Ease)
                .SetLoops(Loops, LoopType);
            base.Play();
        }
    }
}
