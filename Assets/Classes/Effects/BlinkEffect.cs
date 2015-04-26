using System;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Effects
{
    /// <summary>
    /// TODO: complete this shit
    /// </summary>
    public class BlinkEffect : GameEffect
    {
        public FadeEffect.FadeInfo In = new FadeEffect.FadeInfo(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.125f, Ease.Linear );
        public FadeEffect.FadeInfo Out = new FadeEffect.FadeInfo(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.3f, Ease.Linear);

        public static FadeEffect.FadeInfo DefaultIn = new FadeEffect.FadeInfo(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.125f, Ease.Linear);
        public static FadeEffect.FadeInfo DefaultOut = new FadeEffect.FadeInfo(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.3f, Ease.Linear);

        public event EventHandler<GenericEventArgs<BlinkEffect>> BlinkComplete;

        protected virtual void OnBlinkComplete(GenericEventArgs<BlinkEffect> e)
        {
            var handler = BlinkComplete;
            if (handler != null) handler(this, e);
        }

        public event EventHandler InComplete;
        protected virtual void OnInComplete()
        {
            var handler = InComplete;
            if (handler != null) handler(this, EventArgs.Empty);
        }


        public event EventHandler OutComplete;
        protected virtual void OnOutComplete()
        {
            var handler = OutComplete;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void OnInComplete(object sender, GenericEventArgs<FadeEffect.FadeInfo> fi)
        {
            var fadeEffect = Effects.GameEffectsBase.GetEffectInstanceInInstance<FadeEffect>();
            fadeEffect.StartFade(Out);
            fadeEffect.FadeComplete -= OnInComplete;
            fadeEffect.FadeComplete += OnOutComplete;
            OnInComplete();
        }

        private void OnOutComplete(object sender, GenericEventArgs<FadeEffect.FadeInfo> fi)
        {
            var fadeEffect = Effects.GameEffectsBase.GetEffectInstanceInInstance<FadeEffect>();
            fadeEffect.FadeComplete -= OnOutComplete;
            OnOutComplete();
            OnBlinkComplete(new GenericEventArgs<BlinkEffect>(this));
        }

        public void Play()
        {
            var fadeEffect = Effects.GameEffectsBase.GetEffectInstanceInInstance<FadeEffect>();
            fadeEffect.StartFade(In);
            fadeEffect.FadeComplete += OnInComplete;
        }

        public static void PlayInInstance()
        {
            Effects.GameEffectsBase.GetEffectInstanceInInstance<BlinkEffect>().Play();
        }

    }
}
