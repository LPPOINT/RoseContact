using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Implementation.UI
{
    public class UIColorThemeForegroundChameleon : UIChameleon
    {
        public override Color GetTargetColor()
        {
            var c = GameOver.Instance.UIOverlay.color;
            return new Color(c.r, c.g, c.b, 1);
        }

    }
}
