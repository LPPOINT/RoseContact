using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Implementation.UI
{
    public class UINoAdsVisibilityHandler : RoseEntity
    {

        public  void CheckVisibility()
        {
     
            gameObject.SetActive(!(Ads.Instance.IsNoAdsPurchased));
        }

        private void OnEnabled()
        {
            CheckVisibility();
        }


        protected override void Awake()
        {
            GameMessenger.AddListener(Ads.NoAdsPurchasedEventName, CheckVisibility);
            CheckVisibility();
            base.Awake();
        }
    }
}
