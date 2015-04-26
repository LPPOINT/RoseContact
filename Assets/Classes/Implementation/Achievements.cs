using System.Collections.Generic;
using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class Achievements : SingletonEntity<Achievements>
    {
        public void Test()
        {
            GameCenterManager.SubmitAchievement(100, "TestA");
        }


        public List<int> ScoreAchievements;
        public List<int> GamesAchievements;


        public string GetScoreAchievementsIdByScore(int score)
        {
            var index = ScoreAchievements.IndexOf(score);
            if (index < 0) return string.Empty;
            return "a.reach." + (index + 1);
        }
        public string GetPlayAchievementsIdByGames(int games)
        {
            var index = GamesAchievements.IndexOf(games);
            if (index < 0) return string.Empty;
            return "a.play." + (index + 1);
        }

        public const string RunsCountDbKey = "RunsCount";

        public int TotalCurrentRuns
        {
            get { return Game.Instance.Database.GetInt(RunsCountDbKey); }
        }

        public void ProcessRun()
        {
            Game.Instance.Database.SetInt(RunsCountDbKey, TotalCurrentRuns + 1);
            Debug.Log("Processing " + TotalCurrentRuns + " run.");


            var gamesAchievement = GetPlayAchievementsIdByGames(TotalCurrentRuns);
            if (gamesAchievement != string.Empty)
            {
                Debug.Log("Submit achievement " + gamesAchievement);
                GameCenterManager.SubmitAchievement(100, gamesAchievement);
            }



        }

        public void ProcessScore(int score)
        {
            var scoreAchievement = GetScoreAchievementsIdByScore(score);
            if (scoreAchievement != string.Empty)
            {
                Debug.Log("Submit achievement " + scoreAchievement);
                GameCenterManager.SubmitAchievement(100, scoreAchievement);
            }
        }

    }
}
