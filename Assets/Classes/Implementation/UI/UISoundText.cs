using Assets.Classes.Core;
using SmartLocalization;
using UnityEngine.UI;

namespace Assets.Classes.Implementation.UI
{
    public class UISoundText : RoseEntity
    {
        public string SoundEnabledKey = "Settings.SoundEnabled";
        public string SoundDisabledKey = "Settings.SoundDisabled";

        private void UpdateState()
        {
            var str =
       LanguageManager.Instance.GetTextValue(GameSound.Instance.IsMuted ? SoundDisabledKey : SoundEnabledKey);
            GetComponent<Text>().text = str;
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
