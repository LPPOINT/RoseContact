using Assets.Classes.Implementation;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Core
{

    public interface IChameleon
    {
        Color GetTargetColor();
        Color GetCurrentColor();
        void SetCurrentColor(Color color);
    }

    public class Chameleon : RoseEntity, IChameleon
    {

        public virtual Color GetTargetColor()
        {
            return GameCamera.Instance.Camera.backgroundColor;
        }

        public virtual Color GetCurrentColor()
        {
            return GameCamera.Instance.Camera.backgroundColor;
        }

        public virtual void SetCurrentColor(Color color)
        {
            GameCamera.Instance.Camera.backgroundColor = color;
        }

        protected override void Awake()
        {

            ApplyTargetColor();
            base.Awake();
        }
        private void Start()
        {
            ApplyTargetColor();
        }

        private void Update()
        {
            Update(this);
        }

        public void ApplyTargetColor()
        {
            SetCurrentColor(GetTargetColor());
        }

        public static void Update(IChameleon chameleon)
        {
            var tc = chameleon.GetTargetColor();
            var cc = chameleon.GetCurrentColor();

            if (tc != cc)
            {
                chameleon.SetCurrentColor(tc);
            }
        }

    }
}
