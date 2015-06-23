using Assets.Classes.Core;
using Assets.Classes.Foundation.Extensions;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation.UI
{
    public class UIRotate : RoseEntity
    {
        public float Velocity;
        public Ease Ease;

        protected override void Awake()
        {
            transform.DORotate(new Vector3(0, 0, transform.GetRotationZ() + 360f), Velocity, RotateMode.WorldAxisAdd)
                .SetEase(Ease)
                .SetLoops(-1, LoopType.Incremental)
                .SetSpeedBased();
        }
    }
}
