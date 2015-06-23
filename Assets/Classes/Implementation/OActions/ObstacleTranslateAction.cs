using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation.OActions
{
    public class ObstacleTranslateAction : ObstacleAction
    {
        public Vector2 Translation;
        public float TimeOrSpeed;
        public bool IsSpeedBased;
        public Ease Ease;

        public override void Play()
        {
            base.Play();
            transform.DOMove(
                new Vector3(transform.position.x + Translation.x, transform.position.y + Translation.y,
                    transform.position.z), TimeOrSpeed)
                .SetSpeedBased(IsSpeedBased)
                .SetEase(Ease);
        }
    }
}
