using System.Globalization;
using Assets.Classes.Core;
using SmartLocalization;
using UnityEngine.UI;

namespace Assets.Classes.Implementation
{
    public class HighscoreBanner : SingletonEntity<HighscoreBanner>
    {
        public Text UIHighscore;
        public Text UIHighscoreText;
        public bool ForceShow = true;

        public static bool ShouldShow
        {
            get { return Game.ImplementationInstance.HighScore != 0; }
        }

        protected override void Awake()
        {
            if (!ShouldShow && !ForceShow)
            {
                gameObject.SetActive(false);
                return;
            }
            UIHighscoreText.text = LanguageManager.Instance.GetTextValue("HighscoreBanner.BestScore");
            UIHighscore.text = "-" +  Game.ImplementationInstance.HighScore.ToString(CultureInfo.InvariantCulture) + "-";
            base.Awake();
        }
    }
}
