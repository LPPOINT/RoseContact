using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Classes.Core
{
    public class UIPopup : RoseEntity
    {

        public enum PopupState
        {
            Showed,
            Hided,
            Showing,
            Hiding
        }

        public PopupState State { get; private set; }

        public void Initialize()
        {
            State = PopupState.Hided;
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            if(State != PopupState.Showed) State = PopupState.Showing;
            gameObject.SetActive(true);
        }
        public virtual void Hide()
        {
            if (State != PopupState.Hided) State = PopupState.Hiding;
        }


        public virtual void OnShowed()
        {
            State = PopupState.Showed;
            onShowed.Invoke();
        }
        public virtual void OnHided()
        {
            State = PopupState.Hided;
            onHided.Invoke();
            gameObject.SetActive(false);
        }


        public UnityEvent onShowed;
        public UnityEvent onHided;

    }
}
