using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;

namespace Assets.Classes.Implementation.UI
{
    public class UIAspectSpecificGraphic : RoseEntity
    {
        public Aspect TargetAspect;

        protected override void Awake()
        {
            if (AspectBasedValues<object>.DetectAspect() != TargetAspect)
            {
                gameObject.SetActive(false);
            }

            base.Awake();
        }
    }
}
