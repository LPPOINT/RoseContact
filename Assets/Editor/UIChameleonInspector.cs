using Assets.Classes.Core;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(Chameleon), true)]
    public class UIChameleonInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var t = (Chameleon) target;

            if (GUILayout.Button("Update all chameleons"))
            {
                foreach ( var c in FindObjectsOfType<Chameleon>())
                {
                    c.ApplyTargetColor();
                }
            }
            
        }
    }
}
