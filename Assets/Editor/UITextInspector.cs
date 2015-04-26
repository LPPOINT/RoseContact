using Assets.Classes.Core;
using UnityEditor;

namespace Assets.Editor
{
    [CustomEditor(typeof(UIText), true)]
    public class UITextInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
