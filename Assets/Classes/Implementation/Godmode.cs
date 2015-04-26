using Assets.Classes.Core;

namespace Assets.Classes.Implementation
{
    public class Godmode : SingletonEntity<Godmode>
    {
        public static bool IsGodmodeEnabled
        {
            get { return IsExist && Instance.enabled && Instance.gameObject.activeSelf; }
        }
    }
}
