using System;
using System.Text;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Implementation;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.DevTools
{
    public class DevMonitor : RoseEntity
    {

        public Text UIText;

        public bool IsActive { get; private set; }

        private DateTime playDate;
        private Chunk last;

        public void SwitchActiveStatus()
        {
            if (IsActive)
            {
                IsActive = false;
                UIText.enabled = false;
            }
            else if (!IsActive)
            {
                IsActive = true;
                UIText.enabled = true;
            }
        }

        private void Update()
        {
            if (!IsActive) return;

            var b = new StringBuilder();

            if (Gameplay.Instance.IsEnabled)
            {
                var c = Gameplay.Instance.DetectCurrentChunk();
                if (c != null)
                {
                    if (last != c)
                    {
                        UIText.text = c.gameObject.name;
                    }
                    last = c;
                }
            }



        }
        protected override void Awake()
        {
            IsActive = true;
            playDate = DateTime.Now;

        }
    }
}
