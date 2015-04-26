using Assets.Classes.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Implementation.UI
{
    public class UISoundImage : RoseEntity
    {
        public Sprite SoundEnabledSprite;
        public Sprite SoundDisabledSprite;


        private void UpdateState()
        {
            var s = GameSound.Instance.IsMuted ? SoundDisabledSprite : SoundEnabledSprite;
            GetComponent<Image>().sprite = s;
        }

        private void OnEnable()
        {
            GameMessenger.AddListener(GameSound.MuteStateChangedEventName, UpdateState);
            UpdateState();
        }

        private void OnDisable()
        {
            GameMessenger.RemoveListener(GameSound.MuteStateChangedEventName, UpdateState);
        }
    }
}
