using Assets.Classes.Foundation.Enums;

namespace Assets.Classes.Foundation.Interfaces
{
    public interface IVisibility4Handler
    {

        VisibilityState4 CurrentVisibilityState { get; }

        void ShowWithAnimation();
        void ShowImmediate();
        void HideWithAnimation();
        void HideImmediate();
    }
}
