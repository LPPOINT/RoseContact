using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Classes.Foundation.Classes;
using UnityEngine;

namespace Assets.Classes.Implementation.OActions
{
    public class BenjaminPositionTrigger : ObstacleActionTrigger
    {
        public enum BenjaminPositionTriggerMode
        {
            Above,
            Below
        }

        public BenjaminPositionTriggerMode Mode;

        private Vector3 GetCurrentPosition()
        {
            if (Gameplay.Instance.CurrentMode == Gameplay.GameplayMode.Play)
            {
                return Benjamin.Instance.transform.position;
            }
             return GameCamera.Instance.transform.position;
        }

        private void Update()
        {
            if (Mode == BenjaminPositionTriggerMode.Above &&
                GetCurrentPosition().y > transform.position.y)
                    InvokeTrigger();
            if(Mode == BenjaminPositionTriggerMode.Below &&
                GetCurrentPosition().y < transform.position.y)
                    InvokeTrigger();

        }

        private void OnDrawGizmos()
        {
            var chunk = GetComponentInParent<Chunk>();
            if(chunk == null)
                return;

            Gizmos.color = Color.blue;
            var left = chunk.LeftCenter;
            var right = chunk.RightCenter;

            Gizmos.DrawLine(new Vector3(left.x, transform.position.y, chunk.transform.position.z), new Vector3(right.x, transform.position.y, chunk.transform.position.z));

        }
    }
}
