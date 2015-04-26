using System;
using UnityEngine;

namespace Assets.Classes.Core
{
    public interface IGameDatabase
    {
        void SetFloat(string key, float value);
        float GetFloat(string key);

        void SetBool(string key, bool value);
        bool GetBool(string key);

        void SetString(string key, string value);
        string GetString(string key);

        void SetInt(string key, int value);
        int GetInt(string key);

        bool ContainsKey(string key);
        void RemoveKey(string key);
    }

    public class PPGameDatabase : IGameDatabase
    {
        public void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);

        }

        public float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public bool GetBool(string key)
        {
            return PlayerPrefs.GetInt(key) != 0;
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public string GetString(string key)
        {
            return PlayerPrefs.GetString(key, string.Empty);
        }

        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public bool ContainsKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void RemoveKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

    public static class GameDatabaseExtensions
    {
        public static void SetDate(this IGameDatabase db, string key, DateTime? value)
        {
            if(value.HasValue)
            {
                db.SetLong(key, value.Value.ToBinary());
            }
            else
            {
               db.SetLong(key, -1);
            }
        }

        public static DateTime? GetDate(this IGameDatabase db, string key)
        {
            if (!db.ContainsKey(key)) return null;

            var l = db.GetLong(key);

            if (l == -1) return null;

            var d = DateTime.FromBinary(l);
            return d;
        }

        public static void SetLong(this IGameDatabase db, string key, long value)
        {
            var bytes = BitConverter.GetBytes(value);
            db.SetString(key, Convert.ToBase64String(bytes));
        }

        public static long GetLong(this IGameDatabase db, string key)
        {
            var bytes = Convert.FromBase64String(db.GetString(key));
            return BitConverter.ToInt64(bytes, 0);
        }

        public static void SetVersion(this IGameDatabase db, string key, Version version)
        {
            db.SetString(key, version.ToString());
        }

        public static Version GetVersion(this IGameDatabase db, string key)
        {
            if (!db.ContainsKey(key)) return null;
            return new Version(db.GetString(key));
        }
    }

}
