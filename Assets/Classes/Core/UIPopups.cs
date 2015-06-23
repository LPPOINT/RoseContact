using System.Collections.Generic;
using Assets.Classes.Cloud;
using UnityEngine;

namespace Assets.Classes.Core
{
    public class UIPopups : SingletonEntity<UIPopups>
    {

        protected override void Awake()
        {

            DialogPopup.Initialize();
            ToastPopup.Initialize();

        }


        public UIDialogPopup DialogPopup;
        public UIToastPopup ToastPopup;


        private bool IsReadyToShow(UIPopup popup)
        {
            return popup.State == UIPopup.PopupState.Hided;
        }

        public void ShowPopup(UIPopup popup)
        {
            popup.Show();
        }

        public void ShowDialog(string title, string contents, params UIDialogPopup.ActionButton[] buttons)
        {
            if (!IsReadyToShow(DialogPopup))
            {
                ProcessError("ShowDialog(): DialogPopup already showed!");
                return;
            }

            DialogPopup.Title = title;
            DialogPopup.Contents = contents;

            if (buttons.Length >= 1) DialogPopup.Button1 = buttons[0];
            if (buttons.Length >= 2) DialogPopup.Button2 = buttons[1];
            if (buttons.Length >= 3) DialogPopup.Button3 = buttons[2];
            if (buttons.Length >= 4) Log("ShowDialog(): too many action buttons"); 

            ShowPopup(DialogPopup);
        }

        public void ShowToast(string contents, float time)
        {
            if (!IsReadyToShow(ToastPopup))
            {
                ProcessError("ShowDialog(): ToastPopup already showed!");
                return;
            }
            ToastPopup.Contents = contents;
            ToastPopup.Time = time;

            ShowPopup(ToastPopup);
        }
    }
}
