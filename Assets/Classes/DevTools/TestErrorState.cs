using Assets.Classes.Core;

namespace Assets.Classes.DevTools
{
    public class TestErrorState : GameStateBase
    {
        public override void OnGameStateEnter(object model)
        {
            UIPopups.Instance.ShowDialog("ERROR TEST", "Click to error", new UIDialogPopup.ActionButton("Click!", () => ProcessError("TEST ERROR", Logs.LogOutputFlags.ConsoleLog | Logs.LogOutputFlags.Cloud | Logs.LogOutputFlags.Toast)));
        }
    }
}
