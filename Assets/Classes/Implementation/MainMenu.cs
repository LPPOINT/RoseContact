using System.Collections;
using Assets.Classes.Core;
using Assets.Classes.Implementation.UI;
using Soomla.Store;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Implementation
{
    [EntryState]
    public class MainMenu : GameState<MainMenu>
    {

        public Graphic UI;
        public Graphic HowToPlayLoadingGraphic;
        public Animator Animator;

        public UIMainMenuBestScoreText MainMenuBestScoreText;
        public UIHowToPlayActionText HowToPlayActionText;
        public UINoAdsVisibilityHandler NoAdsVisibilityHandler;

        public Color BackgroundColor;


        private Vector3 cameraInitialPosition;

        private bool isShowingAnimationComplete;
        private bool isFirstUpdateInvoked;
        private bool isPlayClicked;
        private bool isHowToPlayWasShowed;

        private float mainMenuHideAnimationStartTime;
        public float MainMenuToGameplayMinDelay = 2f;

        public enum MainMenuSubmenu
        {
            None,
            HowToPlay,
            Settings
        }
        public enum HowToPlayContext
        {
            UserTap,
            BeforeGameplay
        }

        public MainMenuSubmenu CurrentSubmenu { get; private set; }
        public HowToPlayContext CurrentHowToPlayContext { get; private set; }

        private void EnableUI()
        {

            UI.gameObject.SetActive(true);
            HowToPlayLoadingGraphic.gameObject.SetActive(false);
            Animator.enabled = true;
        }

        private void DisableUI()
        {
            Animator.enabled = false;
            UI.gameObject.SetActive(false);
            HowToPlayLoadingGraphic.gameObject.SetActive(false);
        }

        public bool IsTranslatedFromGameover { get; private set; }

        public const string IsHowToPlayShowedEarlierDbKey = "IsHowToPlayShowedEarlier";

        private bool ComputeShouldShowHowToPlayPopupBeforePlay()
        {

            var isShowedEarlier = Game.Instance.Database.GetBool(IsHowToPlayShowedEarlierDbKey);

            Game.Instance.Database.SetBool(IsHowToPlayShowedEarlierDbKey, true);

            if (isShowedEarlier)
                return false;

            return !IsTranslatedFromGameover && !isHowToPlayWasShowed;
        }


        #region Input handlers

        public const string PlayClickEventName = "MMPlayerTap";
        public const string LeaderboardClickEventName = "MMLeaderboard";
        public const string SettingsClickEventName = "MMSettings";
        public const string HowToPlayClickEventName = "MMHowToPlay";
        public const string SettingsCloseEventName = "MMSettingsClose";

        public void OnPlayHit()
        {

            if(!IsValidTap)
                return;

            if (isPlayClicked || !isShowingAnimationComplete || !isFirstUpdateInvoked)
            {
                return;
            }


            isPlayClicked = true;

            if (!ComputeShouldShowHowToPlayPopupBeforePlay())
            {
                mainMenuHideAnimationStartTime = Time.realtimeSinceStartup;
                GameCamera.ImplementationInstance.SetMode(GameCamera.GameCameraMode.Stand);

                if (Gameplay.Instance.IsPreloaded)
                {
                    Animator.Play("MainMenu_Hide");
                }
                else
                {
                    Animator.Play("MainMenu_HideWithLoading");
                }
            }
            else
            {
                isHowToPlayWasShowed = true;
                CurrentSubmenu = MainMenuSubmenu.HowToPlay;
                CurrentHowToPlayContext = HowToPlayContext.BeforeGameplay;
                HowToPlayActionText.SetTextByContext(HowToPlayContext.BeforeGameplay);
                Animator.Play("MainMenu_HowToPlay_Show");
            }
            GameMessenger.Broadcast(PlayClickEventName);
        }

        public void OnRateClick()
        {
            if (!IsValidTap)
                return;
            Social.Instance.Rate();
        }

        public void OnSettingsClick()
        {
            if (!IsValidTap)
                return;
            isShowingAnimationComplete = true;
            Gameplay.Instance.PauseDemo();
            NoAdsVisibilityHandler.CheckVisibility();
            CurrentSubmenu = MainMenuSubmenu.Settings;
            Animator.Play("MainMenu_Settings_Show");
            GameMessenger.Broadcast(SettingsClickEventName);
        }
        public void OnSettingsCloseClick()
        {
            if (!IsValidTap)
                return;
            Gameplay.Instance.ResumeDemo();
            CurrentSubmenu = MainMenuSubmenu.None;
            isPlayClicked = false;
            Animator.Play("MainMenu_Settings_Hide");
            GameMessenger.Broadcast(SettingsCloseEventName);
        }

        public void OnShareClick()
        {
            if (!IsValidTap)
                return;
           Social.Instance.MainMenuShare();
        }

        public void OnSoundClick()
        {
            if (!IsValidTap)
                return;
            GameSound.Instance.ToggleMute();
        }

        public void OnNoAdsClick()
        {
            if (!IsValidTap)
                return;
            Ads.Instance.PurchaseNoAds();
        }

        public void OnRestorePurchasesClick()
        {
            if (!IsValidTap)
                return;
            SoomlaStore.RestoreTransactions();
        }

        public void OnAchievementsClick()
        {
            if (!IsValidTap)
                return;
            GameCenterManager.ShowAchievements();
        }

        public void OnLeaderboardsClick()
        {
            if (!IsValidTap)
                return;
            GameCenterManager.ShowLeaderboard(GameExternals.GCLeaderboardId);
            GameMessenger.Broadcast(LeaderboardClickEventName);
        }

        public void OnHowToPlayClick()
        {
            if (!IsValidTap)
                return;
            isShowingAnimationComplete = true;
            Gameplay.Instance.PauseDemo();
            isHowToPlayWasShowed = true;
            CurrentSubmenu = MainMenuSubmenu.HowToPlay;
            CurrentHowToPlayContext = HowToPlayContext.UserTap;
            HowToPlayActionText.SetTextByContext(HowToPlayContext.UserTap);
            Animator.Play("MainMenu_HowToPlay_Show");
        }

        public void OnHowToPlayActionClick()
        {
            if (!IsValidTap)
                return;
            if (CurrentHowToPlayContext == HowToPlayContext.UserTap)
            {
                Gameplay.Instance.ResumeDemo();
                CurrentSubmenu = MainMenuSubmenu.None;
                Animator.Play("MainMenu_HowToPlay_Hide");
            }
            else
            {
                VisualEffects.ImplementationInstance.PlayMainMenuToGameplayEffects(0.4f);
                Animator.Play("MainMenu_HideHowToPlay");
                StartCoroutine(BeginGameplayDelayed(0.4f));
                isPlayClicked = false;
            }
        }

        public void OnRemoveAdsClick()
        {
            if (!IsValidTap)
                return;
            Ads.Instance.PurchaseNoAds();
        }

        #endregion

        #region Animation event handlers

        public void OnMainMenuShowingComplete()
        {
            isShowingAnimationComplete = true;
        }

        public void OnMainMenuHidingComplete()
        {
            BeginGameplay();
        }

        public void OnSettingsShowingComplete()
        {
            
        }

        public void OnSettingsHidingComplete()
        {
            
        }

        public void OnHowToPlayShowingComplete()
        {
            
        }

        public void OnHowToPlayHidingComplete()
        {
            
        }

        public void OnMainMenuDummyAnimationComplete()
        {
            isBecameVisible = true;
        }

        #endregion

        #region Splash screen close detection

        private bool isBecameVisible;
        private bool isStarted;

        private void OnBecameVisible()
        {
            Debug.Log("BecameVisible");
            isBecameVisible = true;
        }

        public bool IsValidTap
        {
            get { return IsTranslatedFromGameover || IsSplashScreenClosed; }
        }

        public bool IsSplashScreenClosed
        {
            get { return isBecameVisible && isStarted; }
        }

        #endregion

        public const string MainMenuEnteredEventName = "MMEntered";

        private IEnumerator BeginGameplayDelayed(float time)
        {
            yield return new WaitForSeconds(time);
            BeginGameplay();
        }
        private void BeginGameplay()
        {
            DisableUI();

            Gameplay.Instance.DestroyAllActiveChunksInBackground();

            GameCamera.Instance.Camera.transform.position = cameraInitialPosition;
           // Benjamin.Instance.transform.position = benInitialPosition;

            GameCamera.Instance.InvalidateViewport();
            Benjamin.Instance.transform.position = new Vector3(GameCamera.Instance.Viewport.x + GameCamera.Instance.Viewport.width / 2,
    GameCamera.Instance.Viewport.y - GameCamera.Instance.Viewport.height / 2,
    Benjamin.Instance.transform.position.z);

          //  GameCamera.ImplementationInstance.IsBlurEnabled = false;
            GameCamera.ImplementationInstance.SetMode(GameCamera.GameCameraMode.Stand);

            Benjamin.Instance.Show();

            GameStates.Instance.EnableState<Gameplay>(Gameplay.GameplayTranslationContext.FromMainMenu);


        }

        


        public override void OnGameStateEnter(object model)
        {
            isPlayClicked = false;
            isShowingAnimationComplete = false;
            EnableUI();
            CurrentSubmenu = MainMenuSubmenu.None;

           

            IsTranslatedFromGameover = GameStates.Instance.PreviousState == GameOver.Instance;

            if (IsTranslatedFromGameover)
            {
                GetComponent<Animator>().Play("MainMenu_Show");
            }
            else
            {
                GetComponent<Animator>().Play("MainMenu_Dummy");
                OnMainMenuShowingComplete(); // dont play showing animation due to initial lags
            }

            if (IsTranslatedFromGameover)
            {
                Gameplay.Instance.DestroyAllActiveChunksInBackground();

            }

            cameraInitialPosition = GameCamera.Instance.Camera.transform.position;


            Benjamin.Instance.Hide();
            Gameplay.Instance.StartDemo();
            GameCamera.ImplementationInstance.SetMode(GameCamera.GameCameraMode.DemoMovement);

            MainMenuBestScoreText.UpdateBestScoreText();

            GameMessenger.Broadcast(MainMenuEnteredEventName);
        }

        private void Start()
        {
            isStarted = true;
        }

        protected override void UpdateState()
        {
            isFirstUpdateInvoked = true;
            Gameplay.Instance.UpdateChunks();
            base.UpdateState();
        }
    }
}
