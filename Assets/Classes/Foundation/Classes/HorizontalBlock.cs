using System;
using UnityEngine;

namespace Assets.Classes.Foundation.Classes
{
    public class HorizontalBlock : IDisposable
    {
        public HorizontalBlock(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
        }

        public HorizontalBlock(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndHorizontal();
        }
    }
}
