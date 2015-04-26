
using System;
using Assets.Classes.Core;
using GoogleMobileAds.Api;
using SmartLocalization;
using Soomla.Store;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Classes.Implementation
{
    public class Ads : SingletonEntity<Ads>
    {

        #region Purchase
        public class NoAdsGood : LifetimeVG
        {

            public const string NoAdsItemId = "rose.store.noads";

            public NoAdsGood()
                : base("NoAds", "This shit should remove ads", NoAdsItemId, new PurchaseWithMarket(GameExternals.NoAdsProductId, 0.99f))
            {
               
            }
        }


        public const string NoAdsPurchasedEventName = "NoAdsPurchased";

        public void PurchaseNoAds()
        {
            SoomlaStore.BuyMarketItem(GameExternals.NoAdsProductId, "payload-fuckload");
        }

        private const string NoAdsDbKey = "IsAdsRemoved";

        private void InitializeNoAdsPurchaseCallbacks()
        {
            StoreEvents.OnGoodBalanceChanged += (good, balance, amountAdded) =>
            {
                if (good.ItemId == NoAdsGood.NoAdsItemId && amountAdded >= 1)
                {
                    GameMessenger.Broadcast(NoAdsPurchasedEventName);
                }
            };
            StoreEvents.OnSoomlaStoreInitialized += () =>
                                                    {
                                                        if (StoreInventory.GetItemBalance(NoAdsGood.NoAdsItemId) > 0)
                                                        {
                                                            StoreInventory.TakeItem(NoAdsGood.NoAdsItemId, 1);
                                                        }
                                                    };

          
        }

        public bool IsNoAdsPurchased
        {
            get
            { return StoreInventory.GetItemBalance(NoAdsGood.NoAdsItemId) > 0; }
        }


        #endregion


        #region Showing

        private void InitializeAds()
        {
            interstitial = new InterstitialAd(GameExternals.AdMobInterstitialAdUnitId);

            interstitial.AdLoaded += HandleInterstitialLoaded;
            interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
            interstitial.AdOpened += HandleInterstitialOpened;
            interstitial.AdClosing += HandleInterstitialClosing;
            interstitial.AdClosed += HandleInterstitialClosed;
            interstitial.AdLeftApplication += HandleInterstitialLeftApplication;

            LoadNextInterstitial();
        }

        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice(GameExternals.AdMobTestDeviceId)
                .AddKeyword("game")
                .AddKeyword("arcade")
                .Build();
        }


        public float InterstitialInterval = 120;


        private InterstitialAd interstitial;

        private DateTime? lastInterstitialTime;

        private void LoadNextInterstitial()
        {
            interstitial.LoadAd(CreateAdRequest());
        }


        public void RegisterInterstitialRequest()
        {
            lastInterstitialTime = DateTime.Now;
        }
        public bool ShouldShowInterstitial
        {
            get
            {
                if (IsNoAdsPurchased)
                    return false;
                return interstitial.IsLoaded();
                //if (lastInterstitialTime == null)
                //{
                //    return false;
                //}
                //return (DateTime.Now - lastInterstitialTime) > TimeSpan.FromSeconds(InterstitialInterval) && interstitial.IsLoaded();
            }
        }
        public bool ShowInterstitialIfNeeded()
        {


            if (!ShouldShowInterstitial)
            {
                if (lastInterstitialTime == null)
                {
                    RegisterInterstitialRequest();
                }
                return false;
            }

            ShowInterstitial();
            RegisterInterstitialRequest();

            return true;
        }
        private void ShowInterstitial()
        {
            if (interstitial.IsLoaded())
            {
                Debug.Log("!!!!!!!!!!!!!!!!!!Show!!!!!!!!!!!!!!!");
                interstitial.Show();
            }
            else
            {
                Debug.Log("Interstitial is not ready yet.");
            }
        }

        #region Intersitials handlers

        private void print(string s)
        {
            Debug.Log(s);
        }

        public void HandleInterstitialLoaded(object sender, EventArgs args)
        {

        }

        public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {

        }

        public void HandleInterstitialOpened(object sender, EventArgs args)
        {

        }

        void HandleInterstitialClosing(object sender, EventArgs args)
        {

        }

        public void HandleInterstitialClosed(object sender, EventArgs args)
        {
            Debug.Log("!!!!!!!!!!!!!!LOAD NEXT AD!!!!!!!!!!!!!");
            LoadNextInterstitial();
        }

        public void HandleInterstitialLeftApplication(object sender, EventArgs args)
        {

        }

        #endregion

        #endregion

        #region Unity callbacks
        protected override void Awake()
        {
            InitializeAds();
            InitializeNoAdsPurchaseCallbacks();
            base.Awake();
        }

        private void Start()
        {

        }


        #endregion
    }
}
