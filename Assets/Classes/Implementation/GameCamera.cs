using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdvancedInspector;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Foundation.Extensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Classes.Implementation
{

    [AdvancedInspector(true)]
    public class GameCamera : GameCameraImplementationBase<GameCamera>
    {


        #region Aspect based orthographic size


        public AspectBasedFloatValues OrthographicSizes;

        [Inspect, Method(MethodDisplay.Button)]
        public void ApplyAspectBasedOrthographicSize()
        {
            Camera.orthographicSize = OrthographicSizes.GetValueForCurrentAspect();
        }

        #endregion

        #region Follow to  Benjamin

        [Inspect]
        public float FollowDampTime = 0.15f;

        [Inspect, ReadOnly]
        private Vector3 FollowVelocity = Vector3.zero;

        [Inspect, ReadOnly]
        public bool ShouldFollowBenjamin { get; set; }

        public float MinBenjaminYPosition
        {
            get
            {
                var c = Gameplay.Instance.BottomChunk;
                return c.transform.position.y;
            }
        }


        public void UnFollowBenjaminDelayed(float delay)
        {
            StartCoroutine(UnFollowBenjaminDelayedInternal(delay));
        }

        private IEnumerator UnFollowBenjaminDelayedInternal(float delay)
        {
            yield return new WaitForSeconds(delay);
            ShouldFollowBenjamin = false;
        }

        private void UpdateFollowingBenjamin()
        {


            if ( ShouldFollowBenjamin
                && Gameplay.Instance.IsEnabled 
                && Benjamin.Instance != null
                && Benjamin.Instance.transform.position.y > MinBenjaminYPosition 
                && Benjamin.Instance.transform.position.y > Gameplay.Instance.GameplayStartYPosition)
            {
                var point = camera.WorldToViewportPoint(Benjamin.Instance.transform.position);
                var delta = Benjamin.Instance.transform.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                var destination = transform.position + delta;
                var resultPosition = Vector3.SmoothDamp(transform.position, destination, ref FollowVelocity, FollowDampTime);
                if (resultPosition.y > Gameplay.Instance.GameplayStartYPosition)
                    transform.SetPositionY(resultPosition.y);
            }
        }


        #endregion

        #region Demo

        public float DemoVelocity;

        private float cinematicDemoVelocity;

        public bool IsInDemoMovement { get; set; }
        public bool IsInDemoCinematic { get; private set; }


        private void DemoCinematic(float velFrom, float velTo, Ease ease, float time)
        {
            cinematicDemoVelocity = velFrom;

            DOTween.To(() => cinematicDemoVelocity, value => cinematicDemoVelocity = value, velTo, time)
                .SetEase(ease)
                .OnComplete(() =>
                         {
                            IsInDemoCinematic = false;
                         })
                .OnStart(() =>
                         {
                             IsInDemoCinematic = true;
                         });
        }
        public void DemoCinematicStart()
        {
            DemoCinematic(0, DemoVelocity, Ease.InBack, 0.4f);
        }

        public void DemoCinematicStop()
        {
            DemoCinematic(DemoVelocity, 0, Ease.Linear, 0.8f);
        }

        private void UpdateDemoMovement()
        {
           
            if(!IsInDemoMovement && !IsInDemoCinematic)
                return;

            var vel = IsInDemoCinematic ? cinematicDemoVelocity : DemoVelocity;
             transform.Translate(0, vel * Time.deltaTime, 0, Space.World);
        }

        #endregion

        #region Background Color management



        [Inspect]
        public float ChangeBackgroundColorTime;
        [Inspect]
        public Ease ChangeBackgroundColorEase;

        public void ChangeBackgroundColor(Color newColor, float time, Ease ease)
        {
            Camera.DOColor(newColor, time)
                .SetEase(ease);
        }
        public void ChangeBackgroundColor(Color newColor)
        {
            ChangeBackgroundColor(newColor, ChangeBackgroundColorTime, ChangeBackgroundColorEase);
        }

        private void OnColorThemeChanged()
        {
            ChangeBackgroundColor(ColorThemes.Instance.CurrentColorTheme.CameraBackgroundColor);
        }

        #endregion

        #region Align

        public Vector3 DefaultPosition = new Vector3(0, 2.3f, -24f);

        public void AlignTo(Vector3 position)
        {
            transform.position = position;
        }

        [Inspect, Method(MethodDisplay.Button)]
        public void AlignToDefaultPosition()
        {
            AlignTo(DefaultPosition);
        }


        #endregion

        #region Vignette

        private Vignetting vignetting;

        private void InitializeVignetting()
        {
            vignetting = Camera.GetComponent<Vignetting>();
        }

        public bool IsVignetteEnabled
        {
            get { return vignetting.enabled; }
            set { vignetting.enabled = value; }
        }

        #endregion

        #region Mode management

        public enum GameCameraMode
        {
            FolllowBenjamin,
            DemoMovement,
            Stand
        }

        public GameCameraMode LastMode { get; private set; }
        public GameCameraMode CurrentMode { get; private set; }


        private void InitializeMode()
        {
            CurrentMode = GameCameraMode.Stand;
        }

        public void SetMode(GameCameraMode mode)
        {
            LastMode = CurrentMode;
            CurrentMode = mode;
            switch (mode)
            {
                case GameCameraMode.FolllowBenjamin:
                    ShouldFollowBenjamin = true;
                    IsInDemoMovement = false;
                    break;
                case GameCameraMode.DemoMovement:
                    ShouldFollowBenjamin = false;
                    IsInDemoMovement = true;
                    DemoCinematicStart();
                    break;
                case GameCameraMode.Stand:
                    ShouldFollowBenjamin = false;
                    IsInDemoMovement = false;

                    if (LastMode == GameCameraMode.DemoMovement)
                    {
                        DemoCinematicStop();
                    }

                    break;
                default:
                    ShouldFollowBenjamin = false;
                    IsInDemoMovement = false;
                    break;
            }
        }

        #endregion



        #region Unity callbacks
        protected override void Awake()
        {
            AlignToDefaultPosition();
            ApplyAspectBasedOrthographicSize();
            InitializeMode();
            InitializeVignetting();
            GameMessenger.AddListener(ColorThemes.ColorThemeChangedEventName, OnColorThemeChanged);
            base.Awake();
        }

        private void Update()
        {
            UpdateFollowingBenjamin();
            UpdateDemoMovement();
        }

        #endregion

    }
}
