using System;
using Assets.Classes.Effects;
using Assets.Classes.Foundation.Classes;

namespace Assets.Classes.Core
{
    public class FadeTranslation : GameStateTranslation
    {

        public FadeEffect.FadeInfo FadeInfo { get; private set; }

        public override void OnTranslationBegin(object model)
        {
            if (!(model is FadeEffect.FadeInfo))
            {
                ProcessError("Unexpected model passed to FadeTranslation");
                return;
            }
            FadeInfo = model as FadeEffect.FadeInfo;

            var fadeEffect = GameEffectsBase.GetEffectInstanceInInstance<FadeEffect>();
            fadeEffect.StartFade(FadeInfo);
            fadeEffect.FadeComplete += OnFadeComplete;
        }

        private void OnFadeComplete(object sender, GenericEventArgs<FadeEffect.FadeInfo> args)
        {
            Effects.GameEffectsBase.GetEffectInstanceInInstance<FadeEffect>().FadeComplete -= OnFadeComplete;
            if (args.Value == FadeInfo)
            {
                OnTranslationComplete();
            }
        }
    }
}
