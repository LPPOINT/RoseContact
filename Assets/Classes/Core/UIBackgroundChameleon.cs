using Assets.Classes.Implementation;
using UnityEngine;

namespace Assets.Classes.Core
{
    public class UIBackgroundChameleon : UIChameleon
    {
        public override Color GetTargetColor()
        {
            return GameCamera.Instance.Camera.backgroundColor;
        }
    }
}
