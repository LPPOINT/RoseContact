using Assets.Classes.Core;

namespace Assets.Classes.Implementation.OActions
{
    public class PlayerGotInputActionTrigger : ObstacleActionTrigger
    {

        private void OnPlayerGotInput()
        {
            InvokeTrigger();
        }

        protected override void Awake()
        {
            GameMessenger.AddListener(Gameplay.PlayerGotInputEventName, OnPlayerGotInput);
            base.Awake();
        }


        private void OnDestroy()
        {
            GameMessenger.RemoveListener(Gameplay.PlayerGotInputEventName, OnPlayerGotInput);
        }
    }
}
