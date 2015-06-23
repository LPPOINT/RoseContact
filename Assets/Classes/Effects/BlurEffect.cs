using System;
using Assets.Classes.Implementation;
using DG.Tweening;

namespace Assets.Classes.Effects
{
    public class BlurEffect : GameEffect
    {
        [Serializable]
        public class BlurInfo
        {
            public float From;
            public float To;
            public Ease Ease;
            public float Time;

            public BlurInfo()
            {
                
            }

            public BlurInfo(float @from, float to, Ease ease, float time)
            {
                From = @from;
                To = to;
                Ease = ease;
                Time = time;
            }
        }

        public BlurInfo CurrentBlurInfo { get; private set; }


        public void Play(BlurInfo blurInfo)
        {
            Play(blurInfo, GameCamera.Instance.GetComponent<Blur>());
        }
        public void Play(BlurInfo blurInfo, Blur blur)
        {
            DOTween.To(() => blur.blurSize, value => blur.blurSize = value, blurInfo.Time, blurInfo.Time)
                .SetEase(blurInfo.Ease)
                .OnComplete(() =>
                            {
                                CurrentBlurInfo = null;
                            });
        }
    }
}
