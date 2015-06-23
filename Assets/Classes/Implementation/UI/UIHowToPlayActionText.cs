using Assets.Classes.Core;
using SmartLocalization;
using UnityEngine.UI;

namespace Assets.Classes.Implementation.UI
{
    public class UIHowToPlayActionText : RoseEntity
    {
        public string PlayActionKey = "HowToPlay.Play";
        public string BackActionKey = "HowToPlay.Back";

        public void SetTextByContext(MainMenu.HowToPlayContext context)
        {
            var text = GetComponent<Text>();
            if (context == MainMenu.HowToPlayContext.BeforeGameplay)
            {
                text.text = LanguageManager.Instance.GetTextValue(PlayActionKey);
            }
            else
            {
                text.text = LanguageManager.Instance.GetTextValue(BackActionKey);
            }
        }
    }
}
