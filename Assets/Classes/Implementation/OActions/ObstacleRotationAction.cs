using AdvancedInspector;
using UnityEngine;
using Vectrosity;

namespace Assets.Classes.Implementation.OActions
{
    public class ObstacleRotationAction : ObstacleAction
    {

        public float Velocity;
        public Transform Around;

        [Inspect(MethodName = "IsCustomPivotDefined")]
        public bool ShowRotationOrbit;

        [Inspect(MethodName = "IsCustomPivotDefined")]
        public Material OrbitMaterial;

        private bool IsCustomPivotDefined()
        {
            return Around != null;
        }


        private void Update()
        {
            if (State == ObstacleMotionState.Playing)
            {
                if (Around == null)
                {
                    transform.Rotate(0, 0, Velocity * Time.deltaTime);
                }
                else
                {
                    transform.RotateAround(Around.position, Vector3.forward, Velocity * Time.deltaTime);
                }
            }
        }
    }
}
