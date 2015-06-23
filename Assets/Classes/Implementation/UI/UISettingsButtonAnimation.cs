using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Classes.Core;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation.UI
{
    public class UISettingsButtonAnimation : RoseEntity
    {

        public float Time;
        public float Angle;

        private void OnSettingsClick()
        {
            transform.DORotate(
                new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + Angle),
                Time)
                .SetEase(Ease.InOutBack);
        }

        protected override void Awake()
        {
            GameMessenger.AddListener(MainMenu.SettingsClickEventName, OnSettingsClick);
            base.Awake();
        }
    }
}
