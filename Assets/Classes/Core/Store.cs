using Soomla.Store;

namespace Assets.Classes.Core
{
    public class Store : SingletonEntity<Store>
    {


        protected virtual IStoreAssets GetAssets()
        {
            return null;
        }

        private  void Start()
        {
            var assets = GetAssets();
            if (assets == null)
            {
                Logs.Instance.ProcessMessage("Trying to initialize store without store assets");
                return;
            }


            SoomlaStore.Initialize(assets);

        }
    }
}
