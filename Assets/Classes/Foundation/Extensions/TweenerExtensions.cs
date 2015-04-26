using Assets.Classes.Core;
using DG.Tweening;

namespace Assets.Classes.Foundation.Extensions
{
    public static class TweenerExtensions
    {
        public static Tweener SetId(this Tweener tweener, RoseEntity entity, string id)
        {
            return tweener.SetId(entity.MakeUniqueId(id));
        }
    }
}
