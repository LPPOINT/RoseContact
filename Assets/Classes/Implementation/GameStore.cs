using Assets.Classes.Core;
using Soomla.Store;

namespace Assets.Classes.Implementation
{
    public class GameStore : Store
    {

        public static GameStore ImplementationInstance
        {
            get { return Instance as GameStore; }
        }

        public class GameStoreAssets : IStoreAssets
        {
            public int GetVersion()
            {
                return 22;
            }

            public VirtualCurrency[] GetCurrencies()
            {
                return new VirtualCurrency[0];
            }

            public VirtualGood[] GetGoods()
            {
                return new VirtualGood[1]
                       {
                           new Ads.NoAdsGood()
                       };
            }

            public VirtualCurrencyPack[] GetCurrencyPacks()
            {
                return new VirtualCurrencyPack[0];
            }

            public VirtualCategory[] GetCategories()
            {
                return new VirtualCategory[0];
            }
        }

        protected override IStoreAssets GetAssets()
        {
            return new GameStoreAssets();
        }


    }
}
