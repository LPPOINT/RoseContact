using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;
using UnityEngine;

namespace Assets.Classes.Implementation.UI
{
    public class UIAspectSpecificPosition : RoseEntity
    {
        public Aspect TargetAspect;


        public float X;
        public float Y;
        protected override void Awake()
        {
            if (AspectBasedValues<object>.DetectAspect() == TargetAspect)
            {
                var g = GetComponent<RectTransform>();

                g.anchoredPosition = new Vector2(X, Y);

            }
            base.Awake();
        }
    }
}
