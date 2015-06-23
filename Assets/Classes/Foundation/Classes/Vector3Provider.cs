using System;
using UnityEngine;

namespace Assets.Classes.Foundation.Classes
{
    [Serializable]
    public class Vector3Provider
    {

        public Vector3Provider()
        {
            Mode = Vector3ProviderMode.UseAvailableValue;
        }
        public Vector3Provider(Vector3 vectorValue, Transform transformValue, Vector3ProviderMode mode)
        {
            VectorValue = vectorValue;
            TransformValue = transformValue;
            Mode = mode;
        }
        public Vector3Provider(Vector3 v) : this(v, null, Vector3ProviderMode.UseAvailableValue)
        {
            
        }
        public Vector3Provider(Transform t) : this(Vector3.zero, t, Vector3ProviderMode.UseAvailableValue)
        {
            
        }

        public enum Vector3ProviderMode
        {
            ForcedToUseVectorValue,
            ForcedToUseTransformValue,
            UseAvailableValue
        }

        public Vector3 VectorValue;
        public Transform TransformValue;
        public Vector3ProviderMode Mode;

        public void UseVectorValue(Vector3 v)
        {
            Mode = Vector3ProviderMode.ForcedToUseVectorValue;
            VectorValue = v;
        }
        public void UseTransformValue(Transform t)
        {
            Mode = Vector3ProviderMode.ForcedToUseTransformValue;
            TransformValue = t;
        }
        public void UseAvailableValue()
        {
            Mode = Vector3ProviderMode.UseAvailableValue;
        }

        public Vector3 GetVector3()
        {
            if (Mode == Vector3ProviderMode.ForcedToUseTransformValue && TransformValue == null) Mode = Vector3ProviderMode.ForcedToUseVectorValue;

            if (Mode == Vector3ProviderMode.UseAvailableValue && TransformValue != null) return TransformValue.position;
            if (Mode == Vector3ProviderMode.UseAvailableValue && TransformValue == null) return VectorValue;

            if (Mode == Vector3ProviderMode.ForcedToUseTransformValue) return TransformValue.position;
            if (Mode == Vector3ProviderMode.ForcedToUseVectorValue) return VectorValue;

            return VectorValue;

        }

    }
}
