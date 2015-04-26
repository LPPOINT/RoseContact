using UnityEngine;

namespace Assets.Classes.Core
{
    public class FPSMonitoring : SingletonEntity<FPSMonitoring>
    {
        public enum TargetFrameRate
        {
            FrameRate60 = 60,
            FrameRate30 = 30
        }

        public TargetFrameRate FrameRate;

        protected override void Awake()
        {
            Application.targetFrameRate = (int)FrameRate;
        }
        private void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            var fps = 1.0f / deltaTime;
            CurrentFPS = fps;
        }


        private float deltaTime;

        public float CurrentFPS { get; private set; }
    }
}
