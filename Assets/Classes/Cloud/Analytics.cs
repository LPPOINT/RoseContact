using System.Collections.Generic;
using System.Linq;
using Assets.Classes.Core;
using Assets.Classes.Implementation;
using Parse;
using UnityEngine;
using UnityEngine.Cloud.Analytics;

namespace Assets.Classes.Cloud
{
    public class Analytics : SingletonEntity<Analytics>
    {

      

        protected override void Awake()
        {
            Debug.Log("Init analytics");
            UnityAnalytics.StartSDK(GameExternals.UnityAnalyticsId);
            //ParseAnalytics.TrackAppOpenedAsync();
        }


        public  void SendEvent(string n, IDictionary<string, object> data, AnalyticsServices services)
        {
            if (services == AnalyticsServices.ParseDotCom || services == AnalyticsServices.Both)
            {
                var specificData = data.ToDictionary(o => o.Key, o => o.Value.ToString());

                ParseAnalytics.TrackEventAsync(n, specificData);
            }
            if (services == AnalyticsServices.UnityAnalytics || services == AnalyticsServices.Both)
            {
                UnityAnalytics.CustomEvent(n, data);
            }
        }
        public  void SendEvent(string n, IDictionary<string, object> data)
        {
            SendEvent(n, data, AnalyticsServices.Both);
        }
        public  void SendEvent(string n)
        {
            SendEvent(n, new Dictionary<string, object>());
        }

        public  void SendEvent(IAnalyticsEventProvider provider, AnalyticsServices services)
        {
            SendEvent(provider.GetAnalyticsEventName(), provider.GetAnalyticsEventData(), services);
        }
        public  void SendEvent(IAnalyticsEventProvider providers)
        {
            SendEvent(providers, AnalyticsServices.Both);
        }

    }
}
