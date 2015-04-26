


namespace Assets.Classes.Core
{
    public class GameStateTranslation : RoseEntity
    {

        public GameStateBase PreviousState { get; set; }
        public GameStateBase NextState { get; set; }

        protected virtual void OnTranslationComplete()
        {
            GameStates.Instance.RegisterTranslationComplete(this);
        }

 
        public virtual void OnTranslationBegin(object model)
        {
           
        }

    }
}
