using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdvancedInspector;
using Assets.Classes.Cloud;
using Assets.Classes.Effects;
using Assets.Classes.Foundation.Classes;
using DG.Tweening;
using SmartLocalization;
using Soomla.Store;
using UnityEngine;

namespace Assets.Classes.Core
{
    [AdvancedInspector(true)]
    public class GameCore : SingletonEntity<GameCore>
    {
        public IGameDatabase Database { get; private set; }

        protected virtual IGameDatabase InitializeDatabase()
        {
            return new PPGameDatabase();
        }


        public const string GameEntryEvent = "GameEntry";
        public const string GameQuitEvent = "GameQuit";


        #region Callbacks
        protected virtual void OnPreGameInitialized()
        {

        }
        protected virtual void OnPostGameInitialized()
        {

        }
        protected virtual void OnGameStarted()
        {

        }
        protected virtual void OnGameUpdate()
        {

        }

        protected virtual void OnGameExit()
        {
            
        }
        #endregion

        #region Language

        private void InitializeLanguage()
        {
            var langCode = LanguageUtility.GetCurrent2LetterIOSCode();
            Debug.Log("Using language " + langCode);
            LanguageManager.Instance.ChangeLanguage(langCode);
        }

        #endregion


        #region Sessions management

        private const string LastSessionStartDateDbKey = "LastSessionStartDate";
        private const string LastSessionEndDateDbKey = "LastSessionEndDate";
        private const string CurrentSessionDateDbKey = "CurrentSessionDate";
        private const string LastSessionVersionDbKey = "LastSessionVersion";

        protected virtual void OnGameVersionChanged(string oldVersion, string newVersion)
        {

        }

        public DateTime? LastSessionStartDate { get; private set; }
        public DateTime? LastSessionEndDate { get; private set; }
        public TimeSpan? LastSessionDuration { get; private set; }


        public bool IsFirstSession
        {
            get { return LastSessionStartDate == null; }
        }
        public bool IsLastSessionWasUnexpectedShutdowned
        {
            get { return LastSessionStartDate != null && LastSessionEndDate == null; }
        }

        private  void InitializeSessionsManagement()
        {
            LastSessionStartDate = Database.GetDate(LastSessionStartDateDbKey);
            LastSessionEndDate = Database.GetDate(LastSessionEndDateDbKey);

            //if (!Database.ContainsKey(LastSessionVersionDbKey))
            //{
            //    OnGameVersionChanged(string.Empty, PlayerSettings.bundleVersion);
            //}
            //else if(PlayerSettings.bundleVersion != Database.GetString(LastSessionVersionDbKey))
            //{
            //    OnGameVersionChanged(LastSessionVersionDbKey, PlayerSettings.bundleVersion);
            //}

            //Database.SetString(LastSessionVersionDbKey, PlayerSettings.bundleVersion);

            if (LastSessionStartDate != null && LastSessionEndDate != null)
            {
                LastSessionDuration = LastSessionEndDate.Value - LastSessionStartDate.Value;
            }


            Database.SetDate(CurrentSessionDateDbKey, DateTime.Now);
            Database.SetDate(LastSessionStartDateDbKey, DateTime.Now);

        }
        private void ShutdownSessionsManagement()
        {
            Database.SetDate(LastSessionEndDateDbKey, DateTime.Now);
        }

        #endregion

        #region Untiy callbacks
        protected override void Awake()
        {
            //Screen.SetResolution(1952, 2928, false);
            OnPreGameInitialized();


            Database = InitializeDatabase();

            InitializeSessionsManagement();
            InitializeLanguage();

            GameCenterManager.init();
            DOTween.Init();

            GameMessenger.Broadcast(GameEntryEvent);

            OnPostGameInitialized();

        }

        private void Start()
        {
            OnGameStarted();

        }
        private void Update()
        {
            OnGameUpdate();
        }

        private void OnApplicationQuit()
        {
            ShutdownSessionsManagement();
            OnGameExit();
        }

        #endregion

    }
}
