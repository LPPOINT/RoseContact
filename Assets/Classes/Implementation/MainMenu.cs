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
        public Animator Animator;
        public UIHowToPlayActionText HowToPlayActionText;
        public UINoAdsVisibilityHandler NoAdsVisibilityHandler;



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
        }

        private void DisableUI()
        {
            UI.gameObject.SetActive(false);
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

        public void OnPlayHit()
        {
            if(isPlayClicked || !isShowingAnimationComplete || !isFirstUpdateInvoked)
                return;

            isPlayClicked = true;

            if (!ComputeShouldShowHowToPlayPopupBeforePlay())
            {
                mainMenuHideAnimationStartTime = Time.realtimeSinceStartup;
                GameCamera.ImplementationInstance.SetMode(GameCamera.GameCameraMode.Stand);
                Animator.Play("MainMenu_Hide");
            }
            else
            {
                isHowToPlayWasShowed = true;
                CurrentSubmenu = MainMenuSubmenu.HowToPlay;
                CurrentHowToPlayContext = HowToPlayContext.BeforeGameplay;
                HowToPlayActionText.SetTextByContext(HowToPlayContext.BeforeGameplay);
                Animator.Play("MainMenu_HowToPlay_Show");
            }

        }

        public void OnRateClick()
        {
            Social.Instance.Rate();
        }

        public void OnSettingsClick()
        {
            Gameplay.Instance.PauseDemo();
            NoAdsVisibilityHandler.CheckVisibility();
            CurrentSubmenu = MainMenuSubmenu.Settings;
            Animator.Play("MainMenu_Settings_Show");
        }
        public void OnSettingsCloseClick()
        {
            Gameplay.Instance.ResumeDemo();
            CurrentSubmenu = MainMenuSubmenu.None;
            Animator.Play("MainMenu_Settings_Hide");
        }

        public void OnShareClick()
        {
           Social.Instance.MainMenuShare();
        }

        public void OnSoundClick()
        {
            GameSound.Instance.ToggleMute();
        }

        public void OnNoAdsClick()
        {
            Ads.Instance.PurchaseNoAds();
        }

        public void OnRestorePurchasesClick()
        {
            SoomlaStore.RestoreTransactions();
        }

        public void OnAchievementsClick()
        {
            GameCenterManager.ShowAchievements();
        }

        public void OnLeaderboardsClick()
        {
            GameCenterManager.ShowLeaderboard(GameExternals.GCLeaderboardId);
        }

        public void OnHowToPlayClick()
        {
            Gameplay.Instance.PauseDemo();
            isHowToPlayWasShowed = true;
            CurrentSubmenu = MainMenuSubmenu.HowToPlay;
            CurrentHowToPlayContext = HowToPlayContext.UserTap;
            HowToPlayActionText.SetTextByContext(HowToPlayContext.UserTap);
            Animator.Play("MainMenu_HowToPlay_Show");
        }

        public void OnHowToPlayActionClick()
        {
            if (CurrentHowToPlayContext == HowToPlayContext.UserTap)
            {
                Gameplay.Instance.ResumeDemo();
                CurrentSubmenu = MainMenuSubmenu.None;
                Animator.Play("MainMenu_HowToPlay_Hide");
            }
            else
            {
                VisualEffects.ImplementationInstance.PlayMainMenuToGameplayEffects(0.4f);
                Animator.Play("MainMenu_HowToPlay_Hide");
                StartCoroutine(BeginGameplayDelayed(0.4f));
            }
        }

        public void OnRemoveAdsClick()
        {
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

        #endregion

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

            GetComponent<Animator>().Play("MainMenu_Show");

            if (IsTranslatedFromGameover)
            {
                Gameplay.Instance.DestroyAllActiveChunksInBackground();

            }

            cameraInitialPosition = GameCamera.Instance.Camera.transform.position;


           // GameCamera.ImplementationInstance.IsBlurEnabled = true;
            Benjamin.Instance.Hide();
            Gameplay.Instance.StartDemo();
            GameCamera.ImplementationInstance.SetMode(GameCamera.GameCameraMode.DemoMovement);
        }

        protected override void UpdateState()
        {
            isFirstUpdateInvoked = true;
            Gameplay.Instance.UpdateChunks();
            base.UpdateState();
        }
    }
}
