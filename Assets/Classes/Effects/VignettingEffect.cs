using System;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Foundation.Extensions;
using UnityEngine;

namespace Assets.Classes.Effects
{
    public class VignettingEffect : GameEffect
    {
        public Color TargetColor { get; private set; }

        public Color CurrentColor
        {
            get { return Renderer.material.color; }
        }



        public float ConstantAlpha;
        public bool LockAlpha;

        public Renderer Renderer { get; private set; }
        public bool IsRendererExist
        {
            get { return Renderer != null; }
        }

        public void FetchRenderer()
        {
            Renderer = GetComponent<MeshRenderer>();
        }


        public event EventHandler<GenericEventArgs<Color>> ColorTranslationStarted;
        public event EventHandler<GenericEventArgs<Color>> ColorTranslationComplete;

        private void OnITweenColorTranslationStarted()
        {
            var h = ColorTranslationStarted;
            if(h != null) h(this, new GenericEventArgs<Color>(TargetColor));
        }

        private void OnITweenColorTranslationComplete()
        {
            IsInTranslation = false;
            var h = ColorTranslationComplete;
            if (h != null) h(this, new GenericEventArgs<Color>(TargetColor));

        }

        public bool IsInTranslation { get; private set; }

        public void ColorTo(Color target, float time, iTween.EaseType easeType)
        {
            TargetColor = target;

            IsInTranslation = true;

            if (LockAlpha)
            {
                TargetColor = new Color(TargetColor.r, TargetColor.g, TargetColor.b, ConstantAlpha);
            }

            iTween.ColorTo(gameObject, iTween.Hash(
                       "color", TargetColor,
                       "time", time,
                       "easetype", easeType,

                       "onstart", "OnITweenColorTranslationStarted",
                       "oncomplete", "OnITweenColorTranslationComplete"

                ));


        }

        private void Start()
        {
            FetchRenderer();
            Renderer.AlignToCameraViewRect(Camera.main);
        }

    }
}
