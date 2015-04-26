using Assets.Classes.Core;
using SmartLocalization;
using UnityEngine.UI;

namespace Assets.Classes.Implementation.UI
{
    public class UIMainMenuBestScoreText : RoseEntity
    {
        private void Start()
        {
            var t = GetComponent<Text>();
            t.text = string.Format(LanguageManager.Instance.GetTextValue("MainMenu.BestScore"), Game.ImplementationInstance.HighScore);
        }
    }
}
