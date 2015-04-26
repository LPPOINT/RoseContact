using Assets.Classes.Core;

namespace Assets.Classes.Effects
{
    public class GameEffect : RoseEntity
    {


        public static T GetInstanceInScene<T>() where T : GameEffect
        {
            return FindObjectOfType<T>();
        }
    }
}
