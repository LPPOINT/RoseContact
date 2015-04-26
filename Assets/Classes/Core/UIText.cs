
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Core
{
    public class UIText : Text
    {
        public bool CanBeLocalized = true;
        public bool ContinuousCheck = false;
        public string LocalizeableMarker = "$";

        private void CheckForLocalization()
        {
            //if (text.StartsWith(LocalizeableMarker))
            //{
            //    var key = text.Substring(LocalizeableMarker.Length);
            //    var localizedText = LanguageManager.Instance.GetTextValue(key);

            //    if (localizedText == null)
            //    {
            //        Debug.Log("UIText: text value for key '" + key + "' not found for language '" + LanguageManager.Instance.LoadedLanguage + "'");
            //    }
            //    else
            //    {
            //        text = localizedText;
            //    }

            //}
        }

        private string lastText;

        protected override void Start()
        {
            base.Start();
            CheckForLocalization();
        }

        public override void Rebuild(CanvasUpdate update)
        {
            base.Rebuild(update);
            if (update == CanvasUpdate.Layout)
            {
                Debug.Log("update");
                if (text != lastText)
                {
                    CheckForLocalization();
                    lastText = text;
                }
            }
        }
    }
}
