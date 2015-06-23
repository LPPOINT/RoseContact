using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Foundation.Classes
{
    public class TrailRendererSuspender : RoseEntity
    {


        public TrailRenderer TrailRenderer;

        private float time;

        public void Suspend()
        {
            time = TrailRenderer.time;
            Invoke("ResetTrails", 0.01f);
        }

        public void UnSuspend()
        {
            TrailRenderer.time = time;
        }
    }
}
