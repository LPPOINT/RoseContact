using System;
using System.Collections.Generic;
using System.Threading;
using AdvancedInspector;
using Assets.Classes.Cloud;
using Assets.Classes.Core;
using Assets.Classes.Effects;
using Assets.Classes.Foundation.Classes;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    [AdvancedInspector(true)]
    public class Game : GameCore
    {

        #region Implementation

        public Benjamin Benjamin;

        public static Game ImplementationInstance
        {
            get { return Instance as Game; }
        }

        protected override IGameDatabase InitializeDatabase()
        {
            return new PPGameDatabase();
        }


        #endregion

        protected override void OnGameVersionChanged(string oldVersion, string newVersion)
        {
        }

        protected override void OnPostGameInitialized()
        {
            if (Database.ContainsKey("0.7Special") || Database.ContainsKey("0.6Special"))
            {
                Debug.Log("Deleting development player prefs.");
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
        }

        #region Score
        private const string HighscoreDbKey = "Highscore";

        public int HighScore
        {
            get
            {
                return Database.GetInt(HighscoreDbKey); }
        }

        public bool ProcessScore(int score)
        {
            new Thread((() => GameCenterManager.ReportScore(score, GameExternals.GCLeaderboardId))).Start();
            if (score > HighScore)
            {
                Database.SetInt(HighscoreDbKey, score);
            }
            return false;
        }
        #endregion
    }
}
