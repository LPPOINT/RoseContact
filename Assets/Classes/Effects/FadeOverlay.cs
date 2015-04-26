using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Effects
{
    public class FadeOverlay : RoseEntity
    {


        public void AlignToFront()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z - 0.1f);
        }

        public void EnableOverlay()
        {
            renderer.enabled = true;
        }

        public void DisableOverlay()
        {
            renderer.enabled = false;
        }

        public void SetOverlayColor(Color color)
        {
            renderer.material.color = color;
        }
    }
}
