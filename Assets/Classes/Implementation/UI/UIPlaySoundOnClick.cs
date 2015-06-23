using Assets.Classes.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Implementation.UI
{
    public class UIPlaySoundOnClick : RoseEntity
    {
        public AudioClip Sound;
        public Button Button;

        protected override void Awake()
        {
            if (Button == null)
            {
                Button = GetComponent<Button>();
            }
            if(Button == null)
                return;
            Button.onClick.AddListener(() =>
                                       {
                                           GameSound.Instance.PlaySingle(Sound, GameSound.ClipType.Sfx);
                                       });
        }
    }
}
