using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Classes.Core
{
    public class UIDialogPopup : UIFullscreenPopup
    {
        public class ActionButton
        {
            public ActionButton(string text, Action action, bool hidePopupAfterExecute)
            {
                Text = text;
                Action = action;
                HidePopupAfterExecute = hidePopupAfterExecute;
            }

            public ActionButton(string text, Action action)
            {
                Text = text;
                Action = action;
                HidePopupAfterExecute = true;
            }

            public ActionButton(string text)
            {
                Text = text;
                Action = () => { };
                HidePopupAfterExecute = true;
            }


            public string Text;
            public Action Action;
            public bool HidePopupAfterExecute;
        }

        public string Title;
        public string Contents;
        public ActionButton Button1;
        public ActionButton Button2;
        public ActionButton Button3;

        public Button UIButton1;
        public Button UIButton2;
        public Button UIButton3;
        public Text UIButton1Text;
        public Text UIButton2Text;
        public Text UIButton3Text;
        public Text UITitle;
        public Text UIContents;

        public override void Show()
        {

            UITitle.text = Title;
            UIContents.text = Contents;

            BindActionButton(Button1, UIButton1, UIButton1Text);
            BindActionButton(Button2, UIButton2, UIButton2Text);
            BindActionButton(Button3, UIButton3, UIButton3Text);

            base.Show();
        }

        private void BindActionButton(ActionButton button, Button pressHandler, Text text)
        {
            if (button == null)
            {
                pressHandler.gameObject.SetActive(false);
                return;
            }
            pressHandler.gameObject.SetActive(true);
            text.text = button.Text;

        }
        private void ResolveActionButtonClick(ActionButton button)
        {
            button.Action();
            if(button.HidePopupAfterExecute)
                Hide();
        }

        public void OnButton1Click()
        {
            ResolveActionButtonClick(Button1);
        }

        public void OnButton2Click()
        {
            ResolveActionButtonClick(Button2);
        }

        public void OnButton3Click()
        {
            ResolveActionButtonClick(Button3);
        }

    }
}
