using Assets.Classes.Core;
using Assets.Classes.Foundation.Extensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Implementation
{
    public class PlusOne : SingletonEntity<PlusOne>
    {
        public Graphic UIPlusOne;

        public float FallingDistance = 4f;
        public float Time = 0.86f;

        private Sequence currentSequence;

        public void PlayAtPosition(Vector3 position)
        {

            if (currentSequence != null)
            {
                currentSequence.Kill(true);
            }

            UIPlusOne.gameObject.SetActive(true);
            UIPlusOne.color = new Color(UIPlusOne.color.r, UIPlusOne.color.g, UIPlusOne.color.b, 1);
            var s = UIPlusOne.transform.localScale;
            UIPlusOne.transform.localScale = Vector3.zero;

            currentSequence = DOTween.Sequence();

            gameObject.transform.SetPositionXY(position);
            currentSequence.Insert(0,
                 transform.DOMoveY(UIPlusOne.transform.position.y - FallingDistance, Time)
                    .SetEase(Ease.InBack));
            currentSequence.Insert(0,
                UIPlusOne.DOColor(new Color(1, 1, 1, 0), Time - 0.1f)
                    .SetDelay(0.1f));

          currentSequence.Insert(0, UIPlusOne.transform.DOScale(s, 0.4f)
                .SetEase(Ease.OutBack));

            currentSequence.OnComplete(() =>
                                       {
                                           UIPlusOne.gameObject.SetActive(false);
                                           UIPlusOne.color = new Color(UIPlusOne.color.r, UIPlusOne.color.g,
                                               UIPlusOne.color.b, 1);
                                           UIPlusOne.transform.localScale = s;
                                           currentSequence = null;
                                       });
        }
    }
}
