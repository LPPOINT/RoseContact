using System.Collections.Generic;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Extensions;
using Assets.Classes.Implementation.OActions;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class Obstacle : RoseEntity
    {

        public const string Tag = "Obstacle";



        private List<ObstacleAction> motions;
        public List<ObstacleAction> Motions
        {
            get { return motions ?? (motions = new List<ObstacleAction>(GetComponents<ObstacleAction>())); }
        }

        public void PlayCollisionMotion()
        {
            var s = transform.GetScaleX();
            var o = 1.2f;
            var t = 0.18f;
            var es = s*o;

            transform.DOScale(new Vector3(es, es, transform.localScale.z), t)
                .SetEase(Ease.Linear)
                .SetLoops(2, LoopType.Yoyo);

        }
    }
}
