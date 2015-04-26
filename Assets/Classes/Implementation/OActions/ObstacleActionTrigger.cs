using Assets.Classes.Core;

namespace Assets.Classes.Implementation.OActions
{
    public class ObstacleActionTrigger : RoseEntity
    {

        public const string ObstacleActionTriggerInvokedEventName = "ObstacleActionTriggerInvoked";

        public virtual bool IsOneShotTrigger
        {
            get { return true; }
        }

        public void InvokeTrigger()
        {
            GameMessenger.Broadcast(ObstacleActionTriggerInvokedEventName, this);
            if (IsOneShotTrigger)
                enabled = false;
        }
    }
}
