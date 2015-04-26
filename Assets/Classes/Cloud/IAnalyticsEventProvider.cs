using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Cloud.Analytics;

namespace Assets.Classes.Cloud
{
    public interface IAnalyticsEventProvider
    {
        string GetAnalyticsEventName();
        IDictionary<string, object> GetAnalyticsEventData();
    }

    public class DummyAnalyticsEventProvider : IAnalyticsEventProvider
    {
        public string GetAnalyticsEventName()
        {
            return "Dummy";
        }

        public IDictionary<string, object> GetAnalyticsEventData()
        {
            return new Dictionary<string, object>()
                   {
                       {"Date", DateTime.Now.ToString("R")}
                   };
        }
    }

    public static class AnalyticsEventProviderExtensions
    {
        public static void SendCustomEvent(this IAnalyticsEventProvider aep)
        {
            Analytics.Instance.SendEvent(aep);
        }
        public static void SendCustomEvent(this IAnalyticsEventProvider aep, AnalyticsServices services)
        {
            Analytics.Instance.SendEvent(aep, services);
        }
    }

    

}
