using System.Collections.Generic;
using System.Linq;
using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Effects
{
    public class GameEffectsBase : SingletonEntity<GameEffectsBase>
    {

        private static List<GameEffect> DefinedEffects;


        public static T GetEffectInstanceInInstance<T>() where T : GameEffect
        {
            return DefinedEffects.FirstOrDefault(effect => effect.GetType() == typeof (T)) as T;
        }

        protected override void Awake()
        {
            DefinedEffects = new List<GameEffect>(FindObjectsOfType<GameEffect>());
        }
    }

    public abstract class GameEffectsImplementationBase<T> : GameEffectsBase where T : class
    {
        private static T implementationInstance;
        public static T ImplementationInstance
        {
            get { return implementationInstance ?? (implementationInstance = Instance as T); }
        }
    }
}
