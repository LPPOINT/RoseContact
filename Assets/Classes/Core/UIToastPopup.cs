using UnityEngine.UI;

namespace Assets.Classes.Core
{
    public class UIToastPopup : UIPopup
    {
        public string Contents;
        public Text UIContents;
        public float Time;

        public string ShowingAnimation = "Showing";
        public string HidingAnimation = "Hiding";

        public override void Show()
        {
            base.Show();
            UIContents.text = Contents;
            animator.Play(ShowingAnimation);
            OnShowed();
        }

        public override void OnShowed()
        {
            Invoke("HideWithAnimation", Time);
            base.OnShowed();
        }

        private void HideWithAnimation()
        {
            animator.Play(HidingAnimation);
            Invoke("Hide", 0.3f);
        }

        public override void Hide()
        {
            OnHided();
            base.Hide();
        }
    }
}
