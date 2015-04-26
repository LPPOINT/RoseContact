using System;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;
using Assets.Classes.Core;
using Assets.Classes.Effects;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Foundation.Enums;
using Assets.Classes.Foundation.Extensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Classes.Implementation
{
    [AdvancedInspector]
    public class Benjamin : SingletonEntity<Benjamin>
    {
        private static Benjamin instance;
        public new static Benjamin Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Game.ImplementationInstance.Benjamin;
                }
                return instance;
            }
        }

        public enum EditorTabs
        {
            Default,
            Teleportation,
            Movement,
            DirectionalMovement,
            Trails,
            Dying
        }
        public enum State
        {
            None,
            Standing,
            ControlledByPlayer,
            DirectionalMovement,
            Dying
        }


        [Inspect, ReadOnly, Tab(EditorTabs.Default)]
        public const string Tag = "Benjamin";


        public const string StateChangedEventName = "BenStateChanged";

        [Inspect, ReadOnly, Tab(EditorTabs.Default)]
        public State CurrentState { get; private set; }

        private void SetState(State newState, bool alsoFireStateChangedEvent)
        {
            CurrentState = newState;
            if (alsoFireStateChangedEvent)
            {
                GameMessenger.Broadcast(StateChangedEventName);
            }
        }

        #region Physics

        public bool IsPhysicsEnabled
        {
            get { return rigidbody2D.IsAwake(); }
        }

        private void EnablePhysics()
        {
            rigidbody2D.WakeUp();
        }

        private void DisablePhysics()
        {
            rigidbody2D.Sleep();
        }

        #endregion

        #region Movement


        public const string MovementInitializedEventName = "BenMovementInitialized";
        public const string MovementStartedEventName = "BenMovementStarted";
        public const string MovementPausedEventName = "BenMovementPaused";
        public const string MovementResumedEventName = "BenMovementResumed";
        public const string MovementDirectionChangedEventName = "BenMovementDirectionChanged";

        public enum BenjaminMovementDirection
        {
            Left,
            Right,
            None
        }

        public BenjaminMovementDirection MovementDirection
        {
            get
            {
                if (MovementVelocity > 0) return BenjaminMovementDirection.Right;
                if (MovementVelocity < 0) return BenjaminMovementDirection.Left;
                return BenjaminMovementDirection.None;
            }
        }

        public Vector2 ForwardMovement
        {
            get { return transform.forward; }
        }
        public Vector2 RightMovement
        {
            get { return transform.right; }
        }
        public Vector2 LeftMovement
        {
            get { return transform.right * -1; }
        }
        public float HorizontalOffsetFromCenter
        {
            get
            {
                var center = GameCamera.Instance.Camera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0)).x;
                var current = transform.position.x;
                return center - current;
            }
        }

        [Inspect(Priority = -99), Tab(EditorTabs.Movement)]
        public BenjaminMovementDirection InitialMovementDirection;
        [Inspect(Priority = -98), Range(0, 999), Tab(EditorTabs.Movement)]
        public float InitialMovementVelocity;
        [Inspect(Priority = -97), Range(0, 3), Tab(EditorTabs.Movement), Spacing(After = 20)]
        public float InitialMovementRadius;
        [Inspect(Priority = -96), Tab(EditorTabs.Movement), Spacing(After = 20)]
        public float MovementVelocity = 160f;

        [Inspect, ReadOnly, Tab(EditorTabs.Movement)]
        public bool IsInMovement { get; private set; }

        [Inspect, ReadOnly, Tab(EditorTabs.Movement)]
        public Vector3Provider MovementPivot { get; private set; }

        [Inspect, ReadOnly, Tab(EditorTabs.Movement)]
        public float RotationAroundPivot
        {
            get { return transform.localRotation.eulerAngles.z; }
        }

        private void ChangeMovementTrajectory(Vector3 newRotationOrigin)
        {
            
            IsInMovement = true;
            MovementPivot.UseVectorValue(newRotationOrigin);
        }
        private void ChangeMovementTrajectory(float newRotationLenghtMultiplier, HorizontalDirection originRelativeSide)
        {
            var s = originRelativeSide == HorizontalDirection.Left ? (Vector3)LeftMovement : (Vector3)RightMovement;
            ChangeMovementTrajectory(transform.position + (s * newRotationLenghtMultiplier));
        }
        public void ChangeMovementTrajectory()
        {

            if (!isTeleportedToRight)
                MovementVelocity = -MovementVelocity;
            isTeleportedToRight = false;
            ChangeMovementTrajectory(InitialMovementRadius, MovementDirection == BenjaminMovementDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left);
            GameMessenger.Broadcast(MovementDirectionChangedEventName);
        }


        [Inspect(Priority = 18), Spacing(After = 0, Before = 20), Method(MethodDisplay.Button), Tab(EditorTabs.Movement)]
        public void StopMovement()
        {
            IsInMovement = false;
            GameMessenger.Broadcast(MovementPausedEventName);
            SetState(State.Standing, true);
        }

        [Inspect(Priority = 19), Method(MethodDisplay.Button), Tab(EditorTabs.Movement)]
        public void ResumeMovement()
        {
            IsInMovement = true;
            GameMessenger.Broadcast(MovementResumedEventName);
            SetState(State.ControlledByPlayer, true);
        }

        private void InitializeMovement()
        {
            MovementPivot = new Vector3Provider();
            GameMessenger.Broadcast(MovementInitializedEventName);
        }
        public void StartupMovement()
        {


            MovementVelocity = InitialMovementDirection == BenjaminMovementDirection.Left
            ? InitialMovementVelocity
            : -InitialMovementVelocity;


            ChangeMovementTrajectory(InitialMovementRadius, InitialMovementDirection == BenjaminMovementDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left);

            GameMessenger.Broadcast(MovementStartedEventName);
            SetState(State.ControlledByPlayer, true);
        }
        private void UpdateMovement()
        {

            if (!IsInMovement || MovementVelocity == 0f)
                return;

            //rigidbody2D.centerOfMass = transform.InverseTransformVector(MovementPivot.GetVector3());
           // rigidbody2D.MoveRotation(MovementVelocity * Time.deltaTime);
            rigidbody2D.RotateAround(MovementPivot.GetVector3(), Vector3.forward, MovementVelocity * Time.deltaTime);

        }



        #endregion

        #region Directional Movement

        [Inspect, Tab(EditorTabs.DirectionalMovement)]
        public float DirectionalMovementVerticalVelocity;
        [Inspect, Tab(EditorTabs.DirectionalMovement)]
        public float DirectionalMovementHorizontalVelocity;

        public enum DirectionalMovementMode
        {
            Horizontal,
            Vertical
        }

        public enum DirectionalMovementDirection
        {
            Forward,
            Backward
        }

        public DirectionalMovementMode CurreDirectionalMovementMode { get; private set; }
        public DirectionalMovementDirection CurrentDirectionalMovementDirection { get; private set; }

        public void StartDirectionalMovement(DirectionalMovementMode mode, DirectionalMovementDirection direction)
        {
            StopMovement();
            CurreDirectionalMovementMode = mode;
            CurrentDirectionalMovementDirection = direction;
            SetState(State.DirectionalMovement, true);
        }

        private void UpdateDirectionalMovement()
        {
            if (CurrentState == State.DirectionalMovement)
            {
                if (CurreDirectionalMovementMode == DirectionalMovementMode.Vertical)
                    transform.Translate(0, DirectionalMovementVerticalVelocity * Time.deltaTime * ((CurrentDirectionalMovementDirection == DirectionalMovementDirection.Backward) ? -1 : 1), 0, Space.World);
                else
                    transform.Translate(DirectionalMovementHorizontalVelocity * Time.deltaTime * ((CurrentDirectionalMovementDirection == DirectionalMovementDirection.Backward) ? -1 : 1), 0, 0, Space.World);
            }
        }

        #endregion

        #region Dying

        [Inspect, Tab(EditorTabs.Dying)]
        public float DyingFallingVelocity;

        public const string DyingStartedEventName = "BenDyingStarted";
        public const string DyingCompeteEventName = "BenDyingComplete";

        private void CheckOutOfViewport()
        {

            if (CurrentState == State.ControlledByPlayer && transform.position.y < GameCamera.Instance.Viewport.yMin - GameCamera.Instance.Viewport.height)
            {
                StartDying();
            }
        }

        public bool IsOnCameraViewport
        {
            get { return GameCamera.Instance.Viewport.Contains(renderer.bounds.ToRect()); }
        }

        public void StartDying()
        {
            isTeleportedToRight = false;
            StopMovement();
            CutoffAndDisableTail();
            DisablePhysics();
            SetState(State.Dying, true);
            GameMessenger.Broadcast(DyingStartedEventName);
        }

        private void UpdateDying()
        {
            if (CurrentState == State.Dying)
            {
                transform.Translate(0, DyingFallingVelocity * Time.deltaTime, 0, Space.World);
                if (!IsOnCameraViewport)
                {
                    VisualEffects.ImplementationInstance.PlayDyingEffectsEnd();
                    Hide();
                    GameMessenger.Broadcast(DyingCompeteEventName);
                }
            }
        }

        #endregion

        #region Show/Hide

        public void Hide()
        {
            gameObject.SetActive(false);
            DisableTail();
            transform.rotation = Quaternion.Euler(0, 0, 0);
            SetState(State.None, true);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            EnableTail();
            SetState(State.Standing, true);
        }

        #endregion

        #region Preplay standing

        public void StandOnBottomOfViewport()
        {
            SetState(State.Standing, false);
            GameCamera.Instance.InvalidateViewport();
            var vMinY = GameCamera.Instance.Camera.transform.position.y - (GameCamera.Instance.Viewport.height / 2);
            var halfHeight = renderer.bounds.size.y/2;
            var y = vMinY -halfHeight;
            transform.SetPositionY(y);
            transform.SetPositionX(GameCamera.Instance.Viewport.xMin + GameCamera.Instance.Viewport.width/2);
        }

        #endregion

        #region Teleportation

        [Inspect, Tab(EditorTabs.Teleportation)]
        public bool IsTeleportationEnabled;

        [Inspect, Tab(EditorTabs.Teleportation)]
        public float TeleportOffset;

        [Inspect, Angle, Tab(EditorTabs.Teleportation)]
        public float TeleportLeftStartAngle;

        [Inspect, Angle, Tab(EditorTabs.Teleportation)]
        public float TeleportRightStartAngle;

        private bool isTeleportedToRight; // This is sucks

        private void TeleportToSide(HorizontalDirection side)
        {

           // Trail1.Reset(this);

            MakeTrailDuplicateAndAttach(transform.position, side == HorizontalDirection.Left ? HorizontalDirection.Right :  HorizontalDirection.Left);

            if (side == HorizontalDirection.Left)
                transform.localRotation = Quaternion.Euler(0, 0, -90);
            else
            {
                isTeleportedToRight = true;
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            var angle = side == HorizontalDirection.Left ? TeleportLeftStartAngle : TeleportRightStartAngle;
            var offset = side == HorizontalDirection.Left ? TeleportOffset : -TeleportOffset;
            var origin = side == HorizontalDirection.Left
                ? GameCamera.Instance.Viewport.xMin
                : GameCamera.Instance.Viewport.xMax;

            transform.position = new Vector3(origin + offset, transform.position.y, transform.position.z);
            StartDirectionalMovement(DirectionalMovementMode.Horizontal, side == HorizontalDirection.Left ? DirectionalMovementDirection.Forward : DirectionalMovementDirection.Backward);

        }

        private void UpdateTeleportation()
        {
            if(CurrentState == State.Standing || (CurrentState == State.DirectionalMovement && CurreDirectionalMovementMode == DirectionalMovementMode.Vertical))
                return;
            if (!IsTeleportationEnabled)
                return;
            var ct = GameCamera.ImplementationInstance.DetectContainmentType(renderer.bounds.ToRect());
            if (ct == GameCameraBase.ViewportContainmentType.Outside)
            {

                var sideToTeleport = (transform.position.x > GameCamera.Instance.transform.position.x)
                    ? HorizontalDirection.Left : HorizontalDirection.Right;
                TeleportToSide(sideToTeleport);
            }
        }

        #endregion

        #region Trail

        private float initialTrailTime;

        [Inspect, Tab(EditorTabs.Trails)]
        public TrailRenderer Trail1;

        [Inspect, Tab(EditorTabs.Trails)]
        public TrailRenderer Trail2;

        [Inspect, Tab(EditorTabs.Trails)]
        public Transform TailTrailAnchor;

        private TrailRenderer currentTailTrail;

        private GameObject outgoingTrailMover;
        private TrailRenderer currentOutgoingTrail;
        private float currentOutgoingTrailVelocity;
        private Vector3 currentOutgoingTrailStartPosition;

        private float maxOutgoingTrailDistance = 2f;

        private float outgoingTrailDistance
        {
            get
            {
                var x1 = currentOutgoingTrail.transform.position.x;
                var x2 = currentOutgoingTrailStartPosition.x;
                return Math.Abs(x1 - x2);
            }
        }

        private void MakeTrailDuplicateAndAttach(Vector3 teleportationPosition, HorizontalDirection teleportationDirection)
        {
            var duplicateVelocity = teleportationDirection == HorizontalDirection.Right
                ? DirectionalMovementHorizontalVelocity
                : -DirectionalMovementHorizontalVelocity;

            if (currentTailTrail == Trail1)
            {
                currentTailTrail = Trail2;
                currentOutgoingTrail = Trail1;
            }
            else
            {
                currentTailTrail = Trail1;
                currentOutgoingTrail = Trail2;
            }

            SetupAndFireOutgoingTrail(currentOutgoingTrail, teleportationPosition, duplicateVelocity);
            SetupAndAttachTailTrail(currentTailTrail);

        }

        private void SetupAndFireOutgoingTrail(TrailRenderer tr, Vector3 position, float velocity)
        {
            outgoingTrailMover.gameObject.SetActive(true);
            outgoingTrailMover.transform.position = position;

            tr.transform.parent = outgoingTrailMover.transform;
            tr.transform.localPosition = new Vector3(0, 0, 0);

            currentOutgoingTrail = tr;
            currentOutgoingTrailVelocity = velocity;
            currentOutgoingTrailStartPosition = position;
        }
        private void SetupAndAttachTailTrail(TrailRenderer tr)
        {
            currentTailTrail.gameObject.SetActive(true);
            currentTailTrail = tr;
            currentTailTrail.transform.parent = transform;
            currentTailTrail.transform.position = TailTrailAnchor.position;
        }

        private void UpdateOutgoingTrail()
        {
            if(currentOutgoingTrail == null)return;

            if (!currentOutgoingTrail.isVisible || outgoingTrailDistance > maxOutgoingTrailDistance)
            {
                outgoingTrailMover.gameObject.SetActive(false);
                currentOutgoingTrail.gameObject.SetActive(false);
                currentOutgoingTrail = null;
            }
            else
            {
                outgoingTrailMover.transform.Translate(currentOutgoingTrailVelocity * Time.deltaTime, 0, 0, Space.World);
            }
        }

        public void CutoffAndDisableTail()
        {
            const float time = 0.54f;
            const Ease ease = Ease.Linear;

            DOTween.To(() => currentTailTrail.time, value => currentTailTrail.time = value, 0, time)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    DisableTail(true);
                });

        }

        private void InitializeTrail()
        {
            initialTrailTime = Trail1.time;
            outgoingTrailMover = new GameObject("OutgoingTrailMover");
            currentTailTrail = Trail1;
            Trail1.gameObject.SetActive(true);
            Trail2.gameObject.SetActive(false);
        }

        private void UpdateTrail()
        {
            UpdateOutgoingTrail();
        }

        public void DisableTail(bool alsoReset = true)
        {
            currentTailTrail.enabled = false;
        }

        public void EnableTail()
        {
            currentTailTrail.enabled = true;
            currentTailTrail.time = initialTrailTime;
            currentTailTrail.Reset(GameCore.Instance);

        }

        #endregion

        #region Unity callbacks

        public const string ScoreLineCrossedEventName = "ScoreLineCrossed";

        protected override void Awake()
        {
            InitializeMovement();
            InitializeTrail();
            SetState(State.Standing, false);
            base.Awake();
        }

        private void Start()
        {

            GameCamera.ImplementationInstance.ShouldFollowBenjamin = true;
            //StartPreplayMovement();
        }

        private void Update()
        {
            UpdateDirectionalMovement();
            UpdateMovement();
            UpdateDying();
            UpdateTrail();
        }

        private void FixedUpdate()
        {
            
            UpdateTeleportation();
            CheckOutOfViewport();
        }



        private void OnTriggerEnter2D(Collider2D c)
        {

            if (!IsPhysicsEnabled || CurrentState == State.Dying)
                return;


            var scoreLine = ScoreLine.GetScoreLineByLineCollider(c);

            if (scoreLine != null && !scoreLine.IsDisposing)
            {
                GameMessenger.Broadcast(ScoreLineCrossedEventName, c, scoreLine);
            }
            if (c.gameObject.CompareTag(Obstacle.Tag) && !(Godmode.IsGodmodeEnabled))
            {
                var o = c.gameObject.GetComponent<Obstacle>();
                if (o != null)
                {
                    o.PlayCollisionMotion();
                }
                GameSound.Instance.PlaySingle(GameSound.Instance.CrashClip, GameSound.ClipType.Sfx);
                StartDying();
                GameCamera.ImplementationInstance.UnFollowBenjaminDelayed(0.7f);
                VisualEffects.ImplementationInstance.PlayDyingEffectsStart();

            }
            else
            {
                
            }
        }

        #endregion


    }
}
