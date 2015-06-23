using UnityEngine;

namespace Assets.Classes.Implementation.Randomization
{
    public class RandomableScale : ChunkRandomable
    {
        public float MinScale;
        public float MaxScale;

        public enum ScaleShape
        {
            Circle,
            Rectangle
        }

        public ScaleShape Shape = ScaleShape.Circle;

        public override void Randomize()
        {
            var scale = UnityEngine.Random.Range(MinScale, MaxScale);
            transform.localScale = new Vector3(scale, scale);
            base.Randomize();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (Shape == ScaleShape.Circle)
            {
                Gizmos.DrawWireSphere(transform.position, MinScale / 0.2679195f);
                Gizmos.DrawWireSphere(transform.position, MaxScale / 0.2679195f);
            }
            else
            {
                Debug.Log("RECTANGLE SHAPE NOT IMPLEMENTED YET!");
            }
        }
    }
}
