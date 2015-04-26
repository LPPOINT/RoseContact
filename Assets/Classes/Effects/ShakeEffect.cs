using System;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Implementation;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Effects
{
    public class ShakeEffect : GameEffect
    {

        [Serializable]
        public class ShakeInfo
        {

            public static readonly ShakeInfo Default = new ShakeInfo(new Vector3(0.005f, 0.005f, 0), 0.2f);

            public ShakeInfo(Vector3 amount, float time)
            {
                Amount = amount;
                Time = time;
                Delay = 0;
            }

            public Vector3 Amount;
            public float Time;
            public float Delay;
        }


        public event EventHandler<GenericEventArgs<ShakeInfo>> ShakeStarted;
        public event EventHandler<GenericEventArgs<ShakeInfo>> ShakeComplete;

        public bool IsShaking { get; private set; }
        public ShakeInfo ActualShakeInfo { get; private set; }

        private void OnITweenShakingStart()
        {
            IsShaking = true;
            var h = ShakeStarted;
            if(h != null) h(this, new GenericEventArgs<ShakeInfo>(ActualShakeInfo));
        }

        private void OnITweenShakingComplete()
        {
            IsShaking = false;
            var h = ShakeComplete;
            if (h != null) h(this, new GenericEventArgs<ShakeInfo>(ActualShakeInfo));
        }


        public void Shake(ShakeInfo info)
        {

            //GameCamera.Instance.Camera.DOShakePosition(info.Time, info.Amount)
            
            iTween.ShakePosition(GameCamera.Instance.Camera.gameObject, iTween.Hash(
                    "time", info.Time,
                    "amount", info.Amount,

                    "onstarttarget", gameObject,
                    "oncompletetarget", gameObject,
                    "onstart", "OnITweenShakingStart",
                    "oncomplete", "OnITweenShakingComplete",
                    "delay", info.Delay
                ));

            ActualShakeInfo = info;

        }

    }
}
