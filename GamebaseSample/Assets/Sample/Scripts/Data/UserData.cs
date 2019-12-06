using System;
using Toast.Gamebase;
using UnityEngine;

namespace GamebaseSample
{
    public class UserData
    {
        public const string KEY_LEVEL = "toast.gamebase.sample.level";
        private const string KEY_EXP = "toast.gamebase.sample.exp";

        private const int MAX_LEVEL = 100;
        private const int LEVEL_UP_EXP = 150;

        public string Id { get; set; }
        public string IdP { get; set; }

        public int Level
        {
            get; private set;
        }

        public int Exp
        {
            get; private set;
        }

        public UserData()
        {
            Level = PlayerPrefs.GetInt(KEY_LEVEL, 1);
            Exp = PlayerPrefs.GetInt(KEY_EXP, 0);
        }

        public void AddExp(int exp)
        {
            LeaderboardApi.SetSingleUserScore(Leaderboard.FACTOR_SCORE, Id, exp, IdP);

            Exp += exp;

            if (Exp >= LEVEL_UP_EXP)
            {
                if (Level < MAX_LEVEL)
                {
                    LevelUp(Mathf.CeilToInt((float)Exp / LEVEL_UP_EXP));
                    Exp %= LEVEL_UP_EXP;
                }
            }

            PlayerPrefs.SetInt(KEY_EXP, Exp);
        }

        private void LevelUp(int upValue)
        {
            Level += upValue;
            PlayerPrefs.SetInt(KEY_LEVEL, Level);

            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long levelUpTime = (long)(DateTime.UtcNow - epochStart).TotalMilliseconds;

            var levelUpData = new GamebaseRequest.Analytics.LevelUpData(Level, levelUpTime);
            Gamebase.Analytics.TraceLevelUp(levelUpData);
        }
    }
}