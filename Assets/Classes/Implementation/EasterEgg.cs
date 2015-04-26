using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class EasterEgg : RoseEntity
    {
        public int TargetScore;
        public Color NewColor;

        private Color defaultColor;


        private void SetScorePanelColor(Color color)
        {
            Gameplay.Instance.UIScorePanel.color = color;
        }

        private void Colorize()
        {
            SetScorePanelColor(NewColor);
        }

        private void RevertColorization()
        {
            SetScorePanelColor(defaultColor);
        }

        private void OnScoreChanged(int oldScore, int newScore)
        {
            if (newScore == TargetScore && oldScore == (TargetScore - 1))
            {
                Colorize();
            }
            else if (newScore != TargetScore && oldScore == TargetScore)
            {
                RevertColorization();
            }
        }


        private void Start()
        {
            defaultColor = Gameplay.Instance.UIScorePanel.color;
        }

        protected override void Awake()
        {
            GameMessenger.AddListener<int, int>(Gameplay.ScoreChangedEventName, OnScoreChanged);
            base.Awake();
        }
    }
}
