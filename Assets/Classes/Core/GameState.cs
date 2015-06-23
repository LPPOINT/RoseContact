namespace Assets.Classes.Core
{
    public class GameState<T> : GameStateBase where T : GameStateBase
    {
        private static T instance;

        public static bool HasInstance
        {
            get { return instance != null; }
        }

        public static T Instance
        {
            get { return instance ?? (instance = GameStates.Instance.GetState<T>()); }
        }

        protected void FetchInstance()
        {
            instance = GameStates.Instance.GetState<T>();
        }
    }
}
