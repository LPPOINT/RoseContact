using UnityEngine;

namespace Assets.Classes.Implementation.OActions
{
    public class ObstacleLogAction : ObstacleAction
    {

        public string LogString = "LOL TRIGGERS WORK!!1";

        public override void Play()
        {
            Debug.Log(LogString);
            base.Play();
        }
    }
}
