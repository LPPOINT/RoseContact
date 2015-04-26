using System;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Foundation.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Core
{
    public class UIFullscreenPopup : UIPopup
    {
        public Graphic Background;
        public Graphic Foreground;

        public string ShowingAnimationStateName = "Showing";
        public string HidingAnimationStateName = "Hiding";

        public override void Show()
        {
            OnShowed();
            base.Show();
        }
        public override void Hide()
        {
            OnHided();
            base.Hide();
        }

        private void OnHidingAnimationComplete(object sender, EventArgs eventArgs)
        {
            OnHided();
        }
        private void OnShowingAnimationComplete(object sender, EventArgs eventArgs)
        {
           
            OnShowed();
        }

    }
}
