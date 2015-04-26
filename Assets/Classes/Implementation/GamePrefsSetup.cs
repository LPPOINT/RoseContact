using Assets.Classes.Core;
using Soomla.Store;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class GamePrefsSetup : SingletonEntity<GamePrefsSetup>
    {


#if UNITY_EDITOR

        public enum BoolPreference
        {
            True,
            False,
            DontTouch
        }


        public BoolPreference IsAdsRemoved;
        public BoolPreference IsHowToPlayShowedEarlier;
        public BoolPreference IsSoundMuted;

        public int TotalRuns = -1;

        private void ProcessBoolPreference(string prefName, BoolPreference value)
        {
            if(value == BoolPreference.DontTouch) return;
            var boolValue = value == BoolPreference.True;
            Game.Instance.Database.SetBool(prefName, boolValue);
        }


        protected override void Awake()
        {
            ProcessBoolPreference(MainMenu.IsHowToPlayShowedEarlierDbKey, IsHowToPlayShowedEarlier);
            ProcessBoolPreference(GameSound.SoundMutedDbKey, IsSoundMuted);

            if (TotalRuns != -1)
            {
                Game.Instance.Database.SetInt(Achievements.RunsCountDbKey, TotalRuns);
            }

            StoreEvents.OnSoomlaStoreInitialized += () =>
                                                    {
                                                        if (IsAdsRemoved != BoolPreference.DontTouch)
                                                        {

                                                            if (
                                                                StoreInventory.GetItemBalance(Ads.NoAdsGood.NoAdsItemId) >
                                                                0 && IsAdsRemoved == BoolPreference.False)
                                                            {
                                                                StoreInventory.TakeItem(Ads.NoAdsGood.NoAdsItemId, 1);
                                                            }
                                                            else if (StoreInventory.GetItemBalance(Ads.NoAdsGood.NoAdsItemId) == 0 && IsAdsRemoved == BoolPreference.True)
                                                            {
                                                                StoreInventory.GiveItem(Ads.NoAdsGood.NoAdsItemId, 1);
                                                            }
                                                            
                                                        }
                                                    };

        }

#endif
    }
}
