using Assets.Classes.Core;

namespace Assets.Classes.Implementation.OActions
{
    public class ObstacleAction : RoseEntity
    {

        public const string ActionCompleteEventName = "ObstacleActiuoasdashndpo;as";

        public float Delay;

        private Obstacle obstacle;
        public Obstacle Obstacle
        {
            get { return obstacle ?? (obstacle = GetComponent<Obstacle>()); }
        }

        public ObstacleActionTrigger Trigger;

        public enum ObstacleMotionState
        {
            NonStarted,
            Playing,
            Paused
        }
        public ObstacleMotionState State { get; private set; }

        public virtual void Pause()
        {
            State = ObstacleMotionState.Paused;
        }
        public virtual void Resume()
        {
            State = ObstacleMotionState.Playing;
        }
        public virtual void Play()
        {
            State = ObstacleMotionState.Playing;
        }

        protected virtual void NotifyActionComplete()
        {
            GameMessenger.Broadcast(ActionCompleteEventName, this);
        }

        private void OnSomeTriggerInvoked(ObstacleActionTrigger someTrigger)
        {
            if (someTrigger.Equals(Trigger))
            {
                PlayDelayed(Delay);
            }
        }

        private void PlayDelayed(float delay)
        {
            Invoke("Play", delay);
        }

        protected override void Awake()
        {
            State = ObstacleMotionState.NonStarted;

            if (Trigger == null)
            {
                Play();
            }
            else
            {
                GameMessenger.AddListener<ObstacleActionTrigger>(ObstacleActionTrigger.ObstacleActionTriggerInvokedEventName, OnSomeTriggerInvoked);
            }

            base.Awake();
        }

        private void OnDestroy() 
        {
            if (Trigger != null)
            {
                GameMessenger.RemoveListener<ObstacleActionTrigger>(
                    ObstacleActionTrigger.ObstacleActionTriggerInvokedEventName, OnSomeTriggerInvoked);
            }
        }

    }
}
