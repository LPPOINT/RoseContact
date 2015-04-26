using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Core
{
    public class UIChameleon : Chameleon
    {
        public Graphic UI;

        public override Color GetCurrentColor()
        {
            return UI.color;
        }

        public override void SetCurrentColor(Color color)
        {
            UI.color = color;
        }
    }
}
