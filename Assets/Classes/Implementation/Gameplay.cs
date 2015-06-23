using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AdvancedInspector;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Foundation.Extensions;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Classes.Implementation
{

    [AdvancedInspector(true)]
    public class Gameplay : GameState<Gameplay>
    {

        public enum GameplayMode
        {
            Play,
            Demo
        }

        public GameplayMode CurrentMode { get; private set; }

        #region Editor

#if UNITY_EDITOR

        [Inspect, Method(MethodDisplay.Button)]
        public void PopulateChunkPrefabsList()
        {
            ChunksPrefabs.Clear();
            var prefabs = AssetDatabase.LoadAllAssetsAtPath("Assets//Prefabs//Chunks//");
            Debug.Log("Count: " + prefabs.Count());
            foreach (var prefab in prefabs)
            {
                if (prefab is GameObject)
                {
                    var go = prefab as GameObject;
                    var chunk = go.GetComponent<Chunk>();
                    if (chunk != null && !chunk.name.Contains("Initial"))
                    {
                        ChunksPrefabs.Add(chunk);
                    }
                }
            }
            EditorUtility.SetDirty(this);
        }

#endif

        #endregion

        #region UI

        public enum ShowState
        {
            Showing,
            Showed,
            Hiding,
            Hided
        }

        public Canvas UICanvas;

        public Image UIScorePanel;
        public Text UIScoreText;
        public Animator UIScoreAnimator;

        public Image UITapToPlay;

        public RectTransform TapToPlayHided;
        public RectTransform TapToPlayShowed;

        public float TapToPlayShowTime;
        public Ease TapToPlayShowEase;

        public float TapToPlayHideTime;
        public Ease TapToPlayHideEase;

        public ShowState TapToPlayShowState { get; private set; }
        public ShowState ScorePanelShowState { get; private set; }

        public RectTransform ScorePanelHided;
        public RectTransform ScorePanelShowed;

        public float ScorePanelShowTime;
        public Ease ScorePanelShowEase;

        public float ScorePanelHideTime;
        public Ease ScorePanelHideEase;

        private Vector3 tapToPlayInitialScale;

        private bool isPointerOverPauseButton;
        private PointerEventData lastPointerEventDataOverGameplayUI;


        private PointerEventData GetCurrentPointerEventData()
        {
            return lastPointerEventDataOverGameplayUI;
        }

        private IEnumerable<RaycastResult> RaycastWithCurrentPointerEventData(GraphicRaycaster raycaster)
        {
            var ed = GetCurrentPointerEventData();

            if (ed == null)
                return new List<RaycastResult>();
            var res = new List<RaycastResult>();
            raycaster.Raycast(ed, res);

            return res;
        }
        private IEnumerable<RaycastResult> RaycastWithCurrentPointerEventData()
        {
            
            return RaycastWithCurrentPointerEventData(graphicRaycaster);
        }

        public IEnumerable<RaycastResult> Raycast(GraphicRaycaster raycaster, PointerEventData ped)
        {


            if (ped == null)
                return new List<RaycastResult>();
            var res = new List<RaycastResult>();
            raycaster.Raycast(ped, res);

            return res;
        }




        private GraphicRaycaster graphicRaycaster;


        private void InitializeUI()
        {
            
            UIScorePanel.gameObject.SetActive(true);
            UIScorePanel.gameObject.transform.position = ScorePanelHided.transform.position;

            tapToPlayInitialScale = UITapToPlay.rectTransform.localScale;

            graphicRaycaster = UICanvas.GetComponent<GraphicRaycaster>();

            ScorePanelShowState = ShowState.Hided;
            TapToPlayShowState = ShowState.Hided;

            UICanvas.gameObject.SetActive(true);
            UIPausePopup.canvas.gameObject.SetActive(true);
            UIPausePopup.gameObject.SetActive(true);
            UIPausePopup.canvas.GetComponent<GraphicRaycaster>().enabled = false;
            HidePausePopupWithoutAnimation();
        }

        private void TweenUI(RectTransform target, RectTransform from, RectTransform to, float time, float delay,
                        Ease ease, Action onStart, Action onComplete)
        {
            target.position = from.position;
            target.DOMoveY(to.position.y, time)
                .SetUpdate(true)
                .SetEase(ease)
                .SetDelay(delay)
                .OnStart(() =>
                {
                    if (onStart != null) onStart();
                })
                .OnComplete(() =>
                {
                    if (onComplete != null) onComplete();
                });
        }

        private void TweenScorePanel(RectTransform from, RectTransform to, float time, float delay, Ease ease, ShowState startState, ShowState resultState)
        {
            TweenUI(UIScorePanel.rectTransform, from, to, time, delay, ease, () => ScorePanelShowState = startState, () => ScorePanelShowState = resultState);
        }

        private bool isTapToPlayShowedInThisRun;

        public void ShowScorePanel()
        {

            UIScorePanel.gameObject.SetActive(true);
            TweenScorePanel(ScorePanelHided, ScorePanelShowed, ScorePanelShowTime, 0.3f, ScorePanelShowEase, ShowState.Showing, ShowState.Showed);
        }
        public void HideScorePanel()
        {
            TweenScorePanel(ScorePanelShowed, ScorePanelHided, ScorePanelHideTime, 0, ScorePanelHideEase, ShowState.Hiding, ShowState.Hided);
        }

        private void TweenTapToPlay(RectTransform from, RectTransform to, float time, float delay, Ease ease, ShowState startState, ShowState resultState)
        {
            TweenUI(UITapToPlay.rectTransform, from, to, time, delay, ease, () => TapToPlayShowState = startState, () => TapToPlayShowState = resultState);
        }
        public void ShowTapToPlay()
        {
            if (isTapToPlayShowedInThisRun)
            {
                Debug.LogWarning("Tap to play panel already showed in this run.");
                return;
            }
            isTapToPlayShowedInThisRun = true;

            UITapToPlay.gameObject.SetActive(true);
            UITapToPlay.rectTransform.localScale = tapToPlayInitialScale;
            UITapToPlay.transform.position = TapToPlayShowed.transform.position;
            TapToPlayShowState = ShowState.Showed;
        }


        public IEnumerator ShowTapToPlayFreezeSafe()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (isTapToPlayShowedInThisRun)
            {
                Debug.LogWarning("Tap to play panel already showed in this run.");
                yield return null;
            }
            isTapToPlayShowedInThisRun = true;

            UITapToPlay.gameObject.SetActive(true);
            UITapToPlay.rectTransform.localScale = tapToPlayInitialScale;
            UITapToPlay.transform.position = TapToPlayShowed.transform.position;
            TapToPlayShowState = ShowState.Showed;
        }

        public void HideTapToPlay()
        {
            UITapToPlay.rectTransform.DOScale(new Vector3(0, 0, UITapToPlay.rectTransform.localScale.z),
                TapToPlayHideTime)
                .SetEase(TapToPlayHideEase)
                .OnStart(() => TapToPlayShowState = ShowState.Hiding)
                .OnComplete(() =>
                            {
                                TapToPlayShowState = ShowState.Hided;
                                //UITapToPlay.rectTransform.localScale = tapToPlayInitialScale;
                            });
        }



        #endregion

        #region Score

        public const string ScoreChangedEventName = "ScoreChanged";


        private int currentScore;
        public int CurrentScore
        {
            get { return currentScore; }
            set
            {
                if (currentScore == value)
                    return;
                var oldScore = currentScore;
                currentScore = value;
                if (UIScoreText == null)
                {
                    Debug.Log("TextBox for score not found!");
                    return;
                }
                UIScoreText.text = value.ToString(CultureInfo.InvariantCulture);



                GameMessenger.Broadcast(ScoreChangedEventName, oldScore, currentScore);
            }
        }


        public string ScorePanelNumberScaleTrigger = "NumberScale";


        private void InitializeScore()
        {
            CurrentScore = 0;
        }
        private void IncrementScore(bool withScaleAnimation)
        {
            CurrentScore++;
            if (withScaleAnimation)
                UIScoreAnimator.SetTrigger(ScorePanelNumberScaleTrigger);
            Achievements.Instance.ProcessScore(CurrentScore);
        }

        #endregion

        #region Chunks

        public List<Chunk> ChunksPrefabs;
        public List<Chunk> InitialChunksPrefabs;

        public Material ScoreLineMaterial;

        public int ChunkPoolSize = 14;

        public float FirstChunkSpawnOffset = 1f;

        public List<Chunk> ActiveChunks { get; private set; }
        public List<Chunk> PooledChunks { get; private set; }

        public Chunk ChunkToTest;

        public bool IsActiveChunk(Chunk chunk)
        {
            return ActiveChunks.Contains(chunk);
        }
        public bool IsPooledChunk(Chunk chunk)
        {
            return PooledChunks.Contains(chunk);
        }
        public bool IsPooledOrActiveChunk(Chunk chunk)
        {
            return IsPooledChunk(chunk) || IsActiveChunk(chunk);
        }

        public Chunk TopChunk { get; private set; }
        public Chunk BottomChunk { get; private set; }

        public Chunk DetectCurrentChunk()
        {

            foreach (var c in ActiveChunks)
            {
                var by = Benjamin.Instance.transform.position.y;
                var yMin = c.BottomMiddle.y;
                var yMax = c.TopMiddle.y;
                if (by >= yMin && by <= yMax)
                {
                    return c;
                }
            }
            return null;
        }

        public bool IsTopChunkFullyVisibly(float margin)
        {
            
            if (TopChunk == null) return false;
            if (CurrentMode == GameplayMode.Demo)
            {
                return TopChunk.TopMiddle.y < GameCamera.Instance.Viewport.yMax - margin;
            }
            else
            {
                return TopChunk.TopMiddle.y < GameCamera.Instance.Viewport.y + GameCamera.Instance.Viewport.height;
            }
        }
        public bool IsBottomChunkFullyInvisibly(float margin)
        {
            if (BottomChunk == null) return false;

            return BottomChunk.TopMiddle.y < GameCamera.Instance.Viewport.yMin - GameCamera.Instance.Viewport.height - margin;
        }

        public void DestroyAllActiveChunksInBackground()
        {

            for (var i = 0; i < ActiveChunks.Count; i++)
            {
                var c = ActiveChunks[i];
                c.gameObject.SetActive(false);
#if UNITY_EDITOR
                c.gameObject.name += "(DestroyInBg)";
#endif
                Destroy(c.gameObject, i / 2f);
            }

            ActiveChunks.Clear();
        }

        public void PushAllChunksToPool()
        {
            foreach (var c in ActiveChunks)
            {
                c.gameObject.SetActive(false);
                PooledChunks.Add(c);
            }

            ActiveChunks.Clear();
        }

        public Chunk GetNextRandomChunkInstance(bool canBePooled)
        {

            if (CurrentMode == GameplayMode.Play && ChunkToTest != null)
            {
                return InstantiateChunk(ChunkToTest);
            }

            if (PooledChunks.Any() && canBePooled)
            {
                var pooled = PooledChunks[UnityEngine.Random.Range(0, PooledChunks.Count)];
                PooledChunks.Remove(pooled);
#if UNITY_EDITOR
                pooled.gameObject.name += "(Active)";
#endif
                pooled.gameObject.SetActive(true);
                return pooled;
            }
            var newInstance = InstantiateChunk(GetNextRandomChunkPrefab());
#if UNITY_EDITOR
            newInstance.gameObject.name += "(Active)";
#endif

            if (newInstance.name == "Chunk64" && Game.ImplementationInstance.HighScore < 4) // Новые игроки не должны это видеть
            {
                return GetNextRandomChunkInstance(false);
            }

            return newInstance;
        }
        public Chunk GetNextRandomChunkPrefab()
        {
            if (CurrentMode == GameplayMode.Demo)
                return ChunksPrefabs.Random();
            return ChunksPrefabs.NonRecurringRandom();
        }

        public Chunk GetNextRandomInitialChunkInstance(bool fromInitialChunks)
        {
            if (InitialChunksPrefabs == null || !InitialChunksPrefabs.Any() || !fromInitialChunks)
            {
                return GetNextRandomChunkInstance(false);
            }
            return InstantiateChunk(InitialChunksPrefabs.NonRecurringRandom());
        }
        public Chunk InstantiateChunk(Chunk prefab)
        {
            return Instantiate(prefab.gameObject).GetComponent<Chunk>();
        }

        public Chunk GetChunkPrefabByInstance(Chunk instance)
        {
            return ChunksPrefabs.FirstOrDefault(chunk => chunk.Number == instance.Number);
        }


        public Chunk InstantiatePooledChunk(Chunk prefab)
        {
            var c = InstantiateChunk(prefab);
#if UNITY_EDITOR
            c.gameObject.name += "(Pooled)";
#endif
            PooledChunks.Add(c);
            c.gameObject.SetActive(false);
            return c;
        }

        private bool isChunksInitialized ;
        public void InitializeChunks()
        {
            if (isChunksInitialized)
                return;
            ActiveChunks = new List<Chunk>();
            PooledChunks = new List<Chunk>();

            if (ChunkToTest == null)
            {
                for (var i = 0; i < ChunkPoolSize; i++)
                {
                    var chunkPrefab = GetNextRandomChunkPrefab();
                    InstantiatePooledChunk(chunkPrefab);
                }
            }
            else
            {
                InstantiatePooledChunk(ChunkToTest);
            }
            isChunksInitialized = true;
        }

        public void SpawnInitialChunk(bool fromInitialChunks)
        {
            var chunk = GetNextRandomInitialChunkInstance(fromInitialChunks);
            GameCamera.Instance.InvalidateViewport();
            chunk.Positionate(GameCamera.Instance.Viewport.yMin - GameCamera.Instance.Viewport.height + FirstChunkSpawnOffset);
            TopChunk = chunk;
            BottomChunk = chunk;
            ActiveChunks.Add(chunk);
        }

        private void DestroyBottomChunk()
        {
            ActiveChunks.Remove(BottomChunk);
            Destroy(BottomChunk.gameObject);
            BottomChunk = ActiveChunks[0];
        }

        private void QueueDestroyBottomChunk()
        {
            Invoke("DestroyBottomChunk", 1);
        }

        private void CancelQueueDestroyBottomChunk()
        {
            CancelInvoke("DestroyBottomChunk");
        }

        public void UpdateChunks()
        {
            GameCamera.Instance.InvalidateViewport();
            if (IsTopChunkFullyVisibly(0))
            {
                var newChunk = GetNextRandomChunkInstance(true);
                newChunk.Positionate(TopChunk.TopMiddle.y);
                TopChunk = newChunk;
                ActiveChunks.Add(newChunk);
            }
            if (IsBottomChunkFullyInvisibly(0))
            {
                ActiveChunks.Remove(BottomChunk);
                Destroy(BottomChunk.gameObject);
                BottomChunk = ActiveChunks[0];
            }
        }

        #endregion

        #region Input

        public const string PlayerGotInputPartiallyEventName = "PlayerPartiallyGotInput";
        public const string PlayerGotInputFullyEventName = "PlayerFullyPartiallyGotInput";

        public float InputLockTime;

        public bool IsInputLocked { get; private set; }

        private void UnlockInputDelayed(Action inputUnlockedAction)
        {
            StartCoroutine(UnlockInputDelayedCoroutine(inputUnlockedAction));
            IsInputLocked = true;
            Invoke("UnlockInput", InputLockTime);
        }

        private IEnumerator UnlockInputDelayedCoroutine(Action inputUnlockedAction)
        {
            yield return new WaitForSeconds(InputLockTime);

            UnlockInput();
            if (inputUnlockedAction != null) inputUnlockedAction();
        }

        private void UnlockInput()
        {
            IsInputLocked = false;
        }

        private bool IsValidGameplayTap(FingerDownEvent g)
        {
            return IsEnabled && !IsPointerOverPauseButton(g) && !IsPaused && !IsInputLocked;
                // && (Benjamin.Instance.CurrentState == Benjamin.State.ControlledByPlayer || Benjamin.Instance.CurrentState == Benjamin.State.DirectionalMovement || Benjamin.Instance.CurrentState == Benjamin.State.Standing);
        }


        public const string FingerDownEventName = "FingerDown";
        public const string FingerUpEventName = "FingerUp";

        public bool IsFingerDown { get; private set; }

        private void OnFingerDown(FingerDownEvent g)
        {


            if (!IsValidGameplayTap(g))
                return;

            

            if (Benjamin.Instance.CurrentState == Benjamin.State.Standing && TapToPlayShowState == ShowState.Showed)
            {
                GameplayStartYPosition = GameCamera.Instance.transform.position.y;

                Benjamin.Instance.StartDirectionalMovement(Benjamin.DirectionalMovementMode.Vertical, Benjamin.DirectionalMovementDirection.Forward);
                HideTapToPlay();
                ShowScorePanel();
                ShowPauseButton();
                GameMessenger.Broadcast(PlayerGotInputPartiallyEventName);
                UnlockInputDelayed(() => GameMessenger.Broadcast(PlayerGotInputFullyEventName));
            }
            else if (Benjamin.Instance.CurrentState == Benjamin.State.DirectionalMovement)
            {
                Benjamin.Instance.StartupMovement();
                GameCameraImplementationBase<GameCamera>.ImplementationInstance.SetMode(
                    GameCamera.GameCameraMode.FolllowBenjamin);
                SingletonEntity<Benjamin>.Instance.ChangeMovementTrajectory();
            }
            else if (Benjamin.Instance.CurrentState == Benjamin.State.ControlledByPlayer)
            {
                SingletonEntity<Benjamin>.Instance.ChangeMovementTrajectory();

            }

            GameSound.Instance.PlayDirectionChanged();



            IsFingerDown = true;
            GameMessenger.Broadcast(FingerDownEventName, g);

        }
        private void OnFingerUp(FingerUpEvent g)
        {

            IsFingerDown = false;
            GameMessenger.Broadcast(FingerUpEventName, g);
        }

        public void OnPointerDownOverPauseButton(BaseEventData ed)
        {
            isPointerOverPauseButton = true;
            lastPointerEventDataOverGameplayUI = ed as PointerEventData;
        }

        public void OnPointerUpOverPauseButton(BaseEventData ed)
        {
            isPointerOverPauseButton = false;
            lastPointerEventDataOverGameplayUI = ed as PointerEventData;
        }

        #endregion

        #region Pause

        public bool IsPaused { get; private set; }


        public bool IsPointerOverPauseButton(FingerDownEvent g)
        {
            var ped = new PointerEventData(EventSystem.current);
            ped.position = new Vector2(g.Position.x, g.Position.y);
            
            var res = Raycast(graphicRaycaster, ped);

            return res.Any(result => result.gameObject.Equals(UIPausePressArea.gameObject));
        }

        public const string GamePausedEventName = "GamePaused";
        public const string GameResumedEventName = "GameResumed";

        public RectTransform UIPauseTapToResume;
        public RectTransform UIPauseHold;

        public RectTransform PauseTapToResumeHided;
        public RectTransform PauseTapToResumeShowed;

        public float PauseTapToResumeShowTime = 0.3f;
        public Ease PauseTapToResumeShowEase = Ease.OutBack;

        public float PauseTapToResumeHideTime = 0.3f;
        public Ease PauseTapToResumeHideEase = Ease.InBack;

        public RectTransform PauseHoldHided;
        public RectTransform PauseHoldShowed;

        public float PauseHoldShowTime = 0.3f;
        public Ease PauseHoldShowEase = Ease.OutBack;

        public float PauseHoldHideTime = 0.3f;
        public Ease PauseHoldHideEase = Ease.InBack;

        public Image UIPause;
        public Button UIPausePressArea;
        public Image UIPausePopup;
        public Image UIPausePopupEye;

        private Color defaultPauseColor;
        private float defaultTimeScale;
        private float defaultPausePopupAlpha;

        public bool CanPause
        {
            get
            {
                return IsEnabled
                       && !isPausing
                       && UIPause.transform.localScale.x != 0
                       && UIPause.color.a != 0
                       && !IsPaused
                       && Benjamin.Instance.CurrentState != Benjamin.State.Dying;
            }
        }

        private void InitializePause()
        {
            UIPause.gameObject.SetActive(true);
            defaultTimeScale = Time.timeScale;
            defaultPauseColor = UIPause.color;
            defaultPausePopupAlpha = UIPausePopup.color.a;
            UIPause.color = new Color(UIPause.color.r, UIPause.color.g, UIPause.color.b, 0);
        }

        private bool canTapOnPauseButton;

        private void TweenPauseButton(float alphaFrom, float alphaTo, float time, bool canTapAfterComplete)
        {
            canTapOnPauseButton = false;
            UIPause.color = new Color(UIPause.color.r, UIPause.color.g, UIPause.color.b, alphaFrom);
            UIPause.DOFade(alphaTo, time).OnComplete(() =>
                                                     {
                                                         canTapOnPauseButton = canTapAfterComplete;
                                                     });
        }
        private void ShowPauseButton()
        {
            TweenPauseButton(0, defaultPauseColor.a, 0.3f, true);
        }
        private void HidePauseButton()
        {
            TweenPauseButton(defaultPauseColor.a, 0, 0.3f, false);
        }

        private bool isResuming;
        private bool isPausing;

        public void Pause()
        {
            if(!CanPause) 
                return;

            IsPaused = true;
            isPausing = true;
            Time.timeScale = 0;
            ShowPausePopup();

            GameMessenger.Broadcast(GamePausedEventName);
        }
        public void Resume()
        {
            if(!IsPaused || isResuming || isPausing) return;
            isResuming = true;
            HidePausePopup();

        }

        public void OnPauseHit()
        {
            if(!IsPaused && canTapOnPauseButton)
                Pause();
        }
        public void OnResumeHit()
        {
            if(IsPaused)
                Resume();
        }

        public bool IsPausePopupTransparent { get; private set; }
        private void SetPausePopupTransparent(bool transparent)
        {
            IsPausePopupTransparent = transparent;
            UIPausePopup.gameObject.SetActive(!transparent);
            GameCamera.ImplementationInstance.IsVignetteEnabled = transparent;


        }


        private void ShowPausePopup()
        {
            UIPausePopup.canvas.GetComponent<GraphicRaycaster>().enabled = true;
            UIPausePopup.color = new Color(UIPausePopup.color.r, UIPausePopup.color.g, UIPausePopup.color.b, 0);
            UIPausePopup.DOFade(defaultPausePopupAlpha, 0.3f).SetUpdate(true);
            TweenUI(UIPauseTapToResume, PauseTapToResumeHided, PauseTapToResumeShowed, PauseTapToResumeShowTime, 0, PauseTapToResumeShowEase, null, OnPausePopupShowed);
           // TweenUI(UIPauseHold, PauseHoldHided, PauseHoldShowed, PauseHoldShowTime, 0, PauseHoldShowEase, null, null);
        }
        private void HidePausePopup()
        {

            UIPausePopup.color = new Color(UIPausePopup.color.r, UIPausePopup.color.g, UIPausePopup.color.b, defaultPausePopupAlpha);
            UIPausePopup.DOFade(0, 0.3f).SetUpdate(true);
            TweenUI(UIPauseTapToResume, PauseTapToResumeShowed, PauseTapToResumeHided, PauseTapToResumeHideTime, 0, PauseTapToResumeHideEase, null, OnPausePopupHided);
           // TweenUI(UIPauseHold, PauseHoldShowed, PauseHoldHided, PauseHoldHideTime, 0, PauseHoldHideEase, null, OnPausePopupHided);
        }

        private void OnPausePopupHided()
        {
            IsPaused = false;
            isResuming = false;
            UIPausePopup.canvas.GetComponent<GraphicRaycaster>().enabled = false;
            Time.timeScale = defaultTimeScale;
            GameMessenger.Broadcast(GameResumedEventName);
        }
        private void OnPausePopupShowed()
        {
            isPausing = false;
        }

        private void HidePausePopupWithoutAnimation()
        {
            UIPausePopup.color = new Color(UIPausePopup.color.r, UIPausePopup.color.g, UIPausePopup.color.b, 0);
            UIPauseTapToResume.transform.position = PauseTapToResumeHided.transform.position;
           // UIPauseHold.transform.position = PauseHoldHided.transform.position;
        }

        #endregion

        #region Play mode


        public Vector3 RunStartPosition { get; private set; }
        public Vector3 RunCurrentPosition
        {
            get { return Benjamin.Instance.transform.position; }
        }
        public float RunCurrentDistance
        {
            get { return Math.Abs(RunCurrentPosition.y - RunStartPosition.y); }
        }

        public float GameoverDelay;

        public void StartRun()
        {
            isTapToPlayShowedInThisRun = false;
            EnablePhysics();

            if (CurrentTranslationContext == GameplayTranslationContext.FromMainMenu)
            {
                GameCamera.ImplementationInstance.SetNextColorTranslationAnimateable(false);
            }

            ColorThemes.Instance.ChangeColorTheme();

            if (ActiveChunks.Any())
            {
                DestroyAllActiveChunksInBackground();
            }

            GameCamera.ImplementationInstance.SetMode(GameCamera.GameCameraMode.FolllowBenjamin);
            //Benjamin.Instance.StartDirectionalMovement(Benjamin.DirectionalMovementMode.Vertical, Benjamin.DirectionalMovementDirection.Forward);
            Benjamin.Instance.StandOnBottomOfViewport();
            SpawnInitialChunk(true);
            RunStartPosition = Benjamin.Instance.transform.position;
            CurrentMode = GameplayMode.Play;

            InitializeScore();
            StartCoroutine(ShowTapToPlayFreezeSafe());


            
        }


        private void OnBenDied()
        {
            var topChunkPrefab = GetChunkPrefabByInstance(TopChunk);
            if (topChunkPrefab != null)
            {
                ChunksPrefabs.SetAsNotUsed(topChunkPrefab);
            }
            HideScorePanel();
            HidePauseButton();
            CancelQueueDestroyBottomChunk();
          //  GameSound.Instance.PlayCrash();
            Game.ImplementationInstance.ProcessScore(CurrentScore);
            StartCoroutine(OnBenDiedDelayed());
        }

        private void OnBenCrossScoreLine(Collider2D c, ScoreLine line)
        {
            line.Dispose(c, true, true);
            IncrementScore(true);
            GameSound.Instance.PlayLineCrossed();
        }

        private IEnumerator OnBenDiedDelayed()
        {
            yield return new WaitForSeconds(GameoverDelay);
            Achievements.Instance.ProcessRun();
            GameStates.Instance.EnableState<GameOver>(new GameOver.GameoverTranslationContext(CurrentScore));
        }

        #endregion

        #region Demo mode

        public bool IsDemoPaused { get; private set; }

        public void PauseDemo()
        {
            IsDemoPaused = true;
            GameCamera.ImplementationInstance.SetMode(GameCamera.GameCameraMode.Stand);
        }

        public void ResumeDemo()
        {
            IsDemoPaused = false;
            GameCamera.ImplementationInstance.SetMode(GameCamera.GameCameraMode.DemoMovement);
        }

        public void StartDemo()
        {
            DisablePhysics();
            CurrentMode = GameplayMode.Demo;
            InitializeChunks();
            SpawnInitialChunk(false);
        }

        #endregion

        #region Physics

        public bool IsPhysicsDisabled { get; private set; }


        private const float DisabledPhysicsTimeRate = 10;
        private float DefaultPhysicsTimeRate;

        private void InitializePhysics()
        {
            DefaultPhysicsTimeRate = Time.fixedDeltaTime;
        }

        public void EnablePhysics()
        {
            IsPhysicsDisabled = false;
            Time.fixedDeltaTime = DefaultPhysicsTimeRate;
        }

        public void DisablePhysics()
        {
            IsPhysicsDisabled = true;
           Time.fixedDeltaTime = DisabledPhysicsTimeRate;
        }


        #endregion

        #region Preload

        public bool IsPreloaded { get; private set; }

        private void Preload()
        {
            GameOver.Instance.PreloadIfNeeded();
            InitializeUI();
        }

        public void PreloadIfNeeded()
        {
            if (!IsPreloaded)
            {
                IsPreloaded = true;
                Preload();
            }
        }

        #endregion

        #region Unity callbacks & GameState Impl

        public float GameplayStartYPosition;

        public enum GameplayTranslationContext
        {
            FromMainMenu,
            FromGameOverMenu
        }

        public GameplayTranslationContext CurrentTranslationContext { get; private set; }

        public override void OnGameStateEnter(object model)
        {
            PreloadIfNeeded();

            if (model == null || (model is GameplayTranslationContext && (GameplayTranslationContext)model == GameplayTranslationContext.FromMainMenu))
            {
                CurrentTranslationContext = GameplayTranslationContext.FromMainMenu;
            }
            else
            {
                CurrentTranslationContext = GameplayTranslationContext.FromGameOverMenu;
            }


            
            if (CurrentTranslationContext == GameplayTranslationContext.FromMainMenu)
            {
                InitializeChunks();
                StartRun();
            }
            else
            {
                Benjamin.Instance.Show();
                GameCamera.Instance.InvalidateViewport();
                StartRun();
            }
            base.OnGameStateEnter(model);
        }

        protected override void UpdateState()
        {
            UpdateChunks();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DestroyAllActiveChunksInBackground();
                StartRun();
            }

        }

        protected override void Awake()
        {
            InitializePhysics();
            InitializePause();
            GameMessenger.AddListener(Benjamin.DyingCompeteEventName, OnBenDied);
            GameMessenger.AddListener<Collider2D, ScoreLine>(Benjamin.ScoreLineCrossedEventName, OnBenCrossScoreLine);
            base.Awake();
        }

        #endregion



    }
}
