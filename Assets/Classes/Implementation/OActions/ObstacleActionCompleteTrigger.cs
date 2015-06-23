using Assets.Classes.Core;

namespace Assets.Classes.Implementation.OActions
{
    public class ObstacleActionCompleteTrigger : ObstacleActionTrigger
    {
        public ObstacleAction Action;

        private void OnSomeActionComplete(ObstacleAction a)
        {
            if (a.Equals(Action))
            {
                InvokeTrigger();
            }
        }

        protected override void Awake()
        {
            GameMessenger.AddListener<ObstacleAction>(ObstacleAction.ActionCompleteEventName, OnSomeActionComplete);
            base.Awake();
        }
    }
}
