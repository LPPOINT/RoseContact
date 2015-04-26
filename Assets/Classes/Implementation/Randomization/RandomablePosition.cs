using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Foundation.Extensions;
using UnityEngine;

namespace Assets.Classes.Implementation.Randomization
{
    public class RandomablePosition : ChunkRandomable
    {
        [Serializable]
        public class TwoTransforms
        {
            public Transform First;
            public Transform Second;
        }

        public List<Transform> AllowedPointPositions;
        public List<TwoTransforms> AllowedRectPositions;


        private void ApplyRandomPointPosition()
        {
            var pp = AllowedPointPositions.Random();
            transform.position = pp.position;
        }

        private void ApplyRandomRectPosition()
        {
            var rp = AllowedRectPositions.Random();
            transform.position = new Vector3(
                UnityEngine.Random.Range(rp.First.position.x, rp.Second.position.x),
                UnityEngine.Random.Range(rp.First.position.y, rp.Second.position.y),
                transform.position.z
           );
        }

        public override void Randomize()
        {

            if ((AllowedPointPositions == null || !AllowedPointPositions.Any()) &&
                (AllowedRectPositions == null || !AllowedRectPositions.Any()))
            {
                Debug.LogError("RandomablePosition.Randomize(): allowed positions not initialized!");
                return;
            }

            if ((AllowedPointPositions == null || !AllowedPointPositions.Any()))
            {
                ApplyRandomRectPosition();
            }
            else if ((AllowedRectPositions == null || !AllowedRectPositions.Any()))
            {
                ApplyRandomPointPosition();
            }
            else
            {
                if (RandomableBoolUtils.RandomBool())
                {
                    ApplyRandomPointPosition();
                }
                else
                {
                    ApplyRandomRectPosition();
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            if((AllowedPointPositions == null || !AllowedPointPositions.Any()) && (AllowedRectPositions == null || !AllowedRectPositions.Any()))
                return;

            if (AllowedPointPositions != null)
            {
                foreach (var pp in AllowedPointPositions)
                {
                    Gizmos.DrawSphere(pp.transform.position, 0.3f);
                }
            }
            if (AllowedRectPositions != null)
            {
                foreach (var rp in AllowedRectPositions)
                {
                    if(rp.First == null || rp.Second == null) continue;
                    Gizmos.DrawLine(new Vector3(rp.First.position.x,rp.First.position.y), new Vector3(rp.Second.position.x, rp.First.position.y));
                    Gizmos.DrawLine(new Vector3(rp.First.position.x, rp.First.position.y), new Vector3(rp.First.position.x, rp.Second.position.y));
                    Gizmos.DrawLine(new Vector3(rp.Second.position.x, rp.First.position.y), new Vector3(rp.Second.position.x, rp.Second.position.y));
                    Gizmos.DrawLine(new Vector3(rp.First.position.x, rp.Second.position.y), new Vector3(rp.Second.position.x, rp.Second.position.y));
                }
            }
        }

    }
}
