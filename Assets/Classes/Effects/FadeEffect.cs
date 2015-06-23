using System;
using Assets.Classes.Foundation.Classes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Effects
{
    public class FadeEffect : GameEffect
    {



        [Serializable]
        public class FadeInfo
        {
            public Color FromColor;
            public Color ToColor;

            public float Time;
            public Ease EaseType;

            public FadeInfo()
            {
                
            }

            public FadeInfo(Color fromColor, Color toColor, float time, Ease easeType)
            {
                FromColor = fromColor;
                ToColor = toColor;
                Time = time;
                EaseType = easeType;
            }
        }

        public event EventHandler<GenericEventArgs<FadeInfo>> FadeStarted;

        protected virtual void OnFadeStarted(GenericEventArgs<FadeInfo> e)
        {
            var handler = FadeStarted;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<GenericEventArgs<FadeInfo>> FadeComplete;

        protected virtual void OnFadeComplete(GenericEventArgs<FadeInfo> e)
        {
            var handler = FadeComplete;
            if (handler != null) handler(this, e);
        }


        public Graphic Overlay;

        public bool IsFading { get; private set; }
        public FadeInfo LastFadeInfo { get; private set; }

        private int defaultSortOrder;
        private int activeSortOrder = 9999;

        protected override void Awake()
        {
            defaultSortOrder = Overlay.canvas.sortingOrder;
        }


        private void OnFadeComplete()
        {
         //   Overlay.canvas.sortingOrder = defaultSortOrder;
            Overlay.color = Color.clear;
            IsFading = false;
            OnFadeComplete(new GenericEventArgs<FadeInfo>(LastFadeInfo));
        }

        private void OnFadeStarted()
        {
            //Overlay.canvas.sortingOrder = activeSortOrder;
        }


        public void StartFade(FadeInfo fadeInfo)
        {
            if(IsFading) return;
            LastFadeInfo = fadeInfo;

            Overlay.color = fadeInfo.FromColor;
            Overlay.DOColor(fadeInfo.ToColor, fadeInfo.Time)
                .SetEase(fadeInfo.EaseType)
                .OnComplete(OnFadeComplete)
                .OnStart(OnFadeStarted);


            IsFading = true;
            OnFadeStarted(new GenericEventArgs<FadeInfo>(fadeInfo));
        }
    }
}
