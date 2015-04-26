using System;
using System.Collections;
using Assets.Classes.Core;
using DG.Tweening;
using SmartLocalization;
using Soomla.Store;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Implementation
{
    public class GameOver : GameState<GameOver>
    {
        public class GameoverTranslationContext
        {

            public GameoverTranslationContext()
            {
                
            }

            public GameoverTranslationContext(int score)
            {
                Score = score;
            }

            public int Score { get; set; }
        }

        public GameoverTranslationContext CurrentTranslationContext { get; private set; }

        public Color BackgroundColor { get; private set; }
        public Color ForegroundColor { get; private set; }

        public Canvas UICanvas;

        public Image UIOverlay;
        public float UIOverlayStartAlpha;
        public float UIOverlayEndAlpha;
        public float UIOverlayShowTime;
        public Ease UIOverlayShowEase;

        public Text UIScore;
        public Text UIBestScore;


        #region No ads social

        public RectTransform NoAdsNotPuchasedSocialPanel;
        public RectTransform NoAdsPuchasedSocialPanel;

        private void SetupSocialPanel()
        {
            if (Ads.Instance.IsNoAdsPurchased)
            {
                NoAdsPuchasedSocialPanel.gameObject.SetActive(true);
                NoAdsNotPuchasedSocialPanel.gameObject.SetActive(false);
            }
            else
            {
                NoAdsPuchasedSocialPanel.gameObject.SetActive(false);
                NoAdsNotPuchasedSocialPanel.gameObject.SetActive(true);
            }
        }

        private void InitializeNoAdsCallbacks()
        {
            GameMessenger.AddListener(Ads.NoAdsPurchasedEventName, () =>
                                                                   {
                                                                       SetupSocialPanel();
                                                                   });
        }

        #endregion

        #region Tween show/hide

        public RectTransform TweenTarget;

        public RectTransform HidedPosition;
        public RectTransform ShowedPosition;

        public float ShowTime;
        public Ease ShowEase;

        public float HideTime;
        public Ease HideEase;

        private void StartTween(RectTransform from, RectTransform to, float time, Ease ease, Action onComplete, State translationState, State finalState)
        {
            TweenTarget.transform.position = from.position;
            TweenTarget.DOMove(to.position, time)
                .SetEase(ease)
                .OnComplete(() =>
                            {
                                CurrentState = finalState;
                                if (onComplete != null)
                                {
                                    onComplete();
                                }
                            })
                .OnStart(() =>
                             {
                               CurrentState = translationState;
                             });
        }
        private void TweenShow()
        {
            StartTween(HidedPosition, ShowedPosition, ShowTime, ShowEase, OnPopupShowed, State.Showing, State.Showed);
        }
        private void TweenHide()
        {
            StartTween(ShowedPosition, HidedPosition, HideTime, HideEase, OnPopupHided, State.Hiding, State.Hided);
        }

        private void SetPosition(RectTransform pos)
        {
            TweenTarget.position = pos.position;
        }

        #endregion

        #region Input handlers
        public void OnRetryHit()
        {
            if(!CanHandleInput) return;
            VisualEffects.ImplementationInstance.PlayGameoverToGameplayEffects(0.4f);
            Hide(GoToGameoverState);
        }

        public void OnRateHit()
        {
            if (!CanHandleInput) return;
            Social.Instance.Rate();
        }



        public void OnShareHit()
        {
            if (!CanHandleInput) return;
            Social.Instance.GameOverShare(CurrentTranslationContext.Score);
        }

        public void OnRemoveAdsHit()
        {
            if (!CanHandleInput) return;
            Ads.Instance.PurchaseNoAds();
        }

        public void OnFacebookHit()
        {
            
            if (!CanHandleInput) return;
            Social.Instance.GameOverFacebook(CurrentTranslationContext.Score);
        }

        public void OnTwitterHit()
        {
            if (!CanHandleInput) return;
            Social.Instance.GameOverTwitter(CurrentTranslationContext.Score);
        }



        public void OnMainMenuHit()
        {
            if (!CanHandleInput) return;
            VisualEffects.ImplementationInstance.PlayGameoverToMainMenuEffects(HideTime);
            Hide(GoToMainMenuState);

        }

        #endregion

        #region State management

        public enum State
        {
            Undefined = 0,
            Hided,
            Showing,
            Showed,
            Hiding

        }

        public State CurrentState { get; private set; }

        public bool CanHandleInput
        {
            get { return CurrentState == State.Showed; }
        }

        private Action hidedAction;

        public void OnPopupHided()
        {
            if (hidedAction != null)
            {
                hidedAction();
            }
        }

        public void OnPopupShowed()
        {
            
        }

        public IEnumerator ShowBannerIfNeededDelayed()
        {
            yield return new WaitForSeconds(0.4f);
            Ads.Instance.ShowInterstitialIfNeeded();
        }

        #endregion

        public void GoToGameoverState()
        {
           // UIOverlay.gameObject.SetActive(false);
            UIOverlay.color = Color.clear;
            GameStates.Instance.EnableState<Gameplay>(Gameplay.GameplayTranslationContext.FromGameOverMenu);
        }

        public void GoToMainMenuState()
        {
           // UIOverlay.gameObject.SetActive(false);
            UIOverlay.color = Color.clear;
            GameStates.Instance.EnableState<MainMenu>();
        }

        private void Hide(Action onHided)
        {
            
            hidedAction = onHided;
            TweenHide();
        }


        public bool IsPreloaded { get; private set; }

        private void Preload()
        {
            UICanvas.gameObject.SetActive(true);
            UIOverlay.gameObject.SetActive(true);
            UIOverlay.color = Color.clear;
            SetPosition(HidedPosition);
        }

        public void PreloadIfNeeded()
        {
            if (!IsPreloaded)
            {
                IsPreloaded = true;
                Preload();
            }
        }

        protected override void Awake()
        {
            InitializeNoAdsCallbacks();
            CurrentState = State.Undefined;
            base.Awake();
        }

        private void Start()
        {
            
        }

        public override void OnGameStateEnter(object model)
        {
            BackgroundColor = ColorThemes.Instance.CurrentColorTheme.OverlayBackgroundColor;
            ForegroundColor = ColorThemes.Instance.CurrentColorTheme.OverlayForegroundColor;

            UIOverlay.gameObject.SetActive(true);
            UIOverlay.color = new Color(BackgroundColor.r, BackgroundColor.g, BackgroundColor.b, UIOverlayEndAlpha);

         

            var context = model as GameoverTranslationContext;

            UIScore.text = "-" + context.Score + "-";


            CurrentTranslationContext = context;

            if (Game.ImplementationInstance.HighScore != context.Score)
            {
                UIBestScore.text = string.Format(LanguageManager.Instance.GetTextValue("Go.HighScore.NotBest"),
                    Game.ImplementationInstance.HighScore);
            }
            else
            {
                UIBestScore.text = LanguageManager.Instance.GetTextValue("Go.HighScore.Best");
            }

            TweenShow();
            SetupSocialPanel();
            StartCoroutine(ShowBannerIfNeededDelayed());

        }

    }
}
