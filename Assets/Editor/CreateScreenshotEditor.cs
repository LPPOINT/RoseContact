using Assets.Classes.Foundation.Classes;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(CreateScreenshot))]
    public class CreateScreenshotEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var t = (CreateScreenshot) target;
            if (GUILayout.Button("Create"))
            {
                t.Create();
            }
        }
    }
}
