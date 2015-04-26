using System.Runtime.InteropServices;
using Assets.Classes.Effects;
using DG.Tweening;
using UnityEngine;


namespace Assets.Classes.Implementation
{
    public class VisualEffects : GameEffectsImplementationBase<VisualEffects>
    {
        public void PlayDyingEffectsStart()
        {
            GetEffectInstanceInInstance<ShakeEffect>().Shake(new ShakeEffect.ShakeInfo(new Vector3(0.1f, 0.1f, 0), 0.2f));
            //GetEffectInstanceInInstance<BlinkEffect>().Play();
        }
        public void PlayDyingEffectsEnd()
        {
            GetEffectInstanceInInstance<ShakeEffect>().Shake(new ShakeEffect.ShakeInfo(new Vector3(0.1f, 0.1f, 0), 0.2f)
                                                             {
                                                                 Delay = 0.2f
                                                             });
            //GetEffectInstanceInInstance<BlinkEffect>().Play();
        }

        public void PlayGameoverToGameplayEffects(float time)
        {
            var blink = GetEffectInstanceInInstance<BlinkEffect>();
            var nextTheme = ColorThemes.Instance.NextColorTheme;
            var currentTheme = ColorThemes.Instance.CurrentColorTheme;
            var end = new Color(currentTheme.OverlayBackgroundColor.r, currentTheme.OverlayBackgroundColor.g,
                currentTheme.OverlayBackgroundColor.b, 1);

            blink.In = new FadeEffect.FadeInfo(new Color(1, 1, 1, 0), end, time, Ease.Linear);
            blink.Out = new FadeEffect.FadeInfo(end, new Color(1, 1, 1, 0), 0.9f, Ease.Linear);

            blink.Play();

        }

        public void PlayGameoverToMainMenuEffects(float time)
        {
            var blink = GetEffectInstanceInInstance<BlinkEffect>();
            var end = Color.black;

            blink.In = new FadeEffect.FadeInfo(new Color(0, 0, 0, 0), end, time, Ease.Linear);
            blink.Out = new FadeEffect.FadeInfo(end, new Color(0, 0, 0, 0), 0.4f, Ease.Linear);

            blink.Play();
        }

        public void PlayMainMenuToGameplayEffects(float time)
        {
            var blink = GetEffectInstanceInInstance<BlinkEffect>();
            var end = Color.black;

            blink.In = new FadeEffect.FadeInfo(new Color(0, 0, 0, 0), end, time, Ease.Linear);
            blink.Out = new FadeEffect.FadeInfo(end, new Color(0, 0, 0, 0), 0.2f, Ease.Linear);

            blink.Play();
        }
    }
}
