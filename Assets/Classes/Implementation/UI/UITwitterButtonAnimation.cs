using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Extensions;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation.UI
{
    public class UITwitterButtonAnimation : RoseEntity
    {


        public float Angle;
        public float Time;

        private Tweener currentTweener;

        private void StartRotation()
        {
            currentTweener = transform.DORotate(
                new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + Angle),
                Time)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }


        protected override void Awake()
        {
            StartRotation();
           
            base.Awake();
        }

    }
}
