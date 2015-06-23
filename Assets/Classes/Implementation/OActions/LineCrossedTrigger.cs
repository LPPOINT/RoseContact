using System.Collections.Generic;
using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Implementation.OActions
{
    public class LineCrossedTrigger : ObstacleActionTrigger
    {

        public List<ScoreLine> TargetLines; 

        private void OnLineCrossed(Collider2D c, ScoreLine l)
        {
            if (TargetLines.Contains(l))
            {
                InvokeTrigger();
            }
        }

        private void OnEnable()
        {
            GameMessenger.AddListener<Collider2D, ScoreLine>(Benjamin.ScoreLineCrossedEventName, OnLineCrossed);
        }

        private void OnDisable()
        {
            GameMessenger.RemoveListener<Collider2D, ScoreLine>(Benjamin.ScoreLineCrossedEventName, OnLineCrossed);
        }
    }
}
