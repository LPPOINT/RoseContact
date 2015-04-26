using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Classes.Core
{
    public  class GameStates : SingletonEntity<GameStates>
    {

        public GameStateBase InitialState { get; private set; }

        protected override void Awake()
        {
            States = new List<GameStateBase>(FindObjectsOfType<GameStateBase>());
            Translations = new List<GameStateTranslation>(FindObjectsOfType<GameStateTranslation>());
            InitialState = States.FirstOrDefault(s => s.GetType().GetCustomAttributes(typeof (EntryStateAttribute), false).Any());

            if (InitialState == null)
            {
                if (!States.Any())
                {
                    Logs.Instance.ProcessFatalError("Sate based game should contains at least one game state!");
                }
                else
                {
                    Logs.Instance.ProcessMessage("Initial game state not found. Using '" + States.FirstOrDefault().GetType().Name + "' state");
                    InitialState = States.FirstOrDefault();
                }

            }

        }


        private void Start()
        {
            if (InitialState != null)
            {
                EnableState(InitialState, null);
            }
        }

        public bool IsInTranslation
        {
            get { return currentTranslation != null; }
        }

        public  GameStateBase CurrentState { get; private set; }
        public  GameStateBase PreviousState { get; private set; }


        public List<GameStateBase> States { get; private set; }
        public List<GameStateTranslation> Translations { get; private set; }


        private GameStateTranslation currentTranslation;
        private object nextStateModel;
        private GameStateBase nextState;

        public void EnableState(GameStateBase state, object model)
        {
            if (CurrentState != null)
            {
                CurrentState.OnGameStateLeave();
                PreviousState = CurrentState;
               // PreviousState.enabled = false;
            }
            CurrentState = state;
            //CurrentState.enabled = true;
            CurrentState.OnGameStateEnter(model);
        }
        public void EnableState(Type stateType, object model)
        {
            var s = States.FirstOrDefault(controller => controller.GetType() == stateType);
            if (s != null)
            {
                EnableState(s, model);
            }
            else
            {
                Debug.LogError("Wrong state type passed");
            }
        }
        public void EnableState<T>(object model) where T : GameStateBase
        {
            EnableState(typeof(T), model);
        }
        public void EnableState<T>() where T : GameStateBase
        {
            EnableState<T>(null);
        }

        public void EnableStateWithTranslation(GameStateBase state, GameStateTranslation translation, object stateModel,
            object translationModel)
        {
            if (IsInTranslation)
            {
                Logs.Instance.ProcessMessage("Already in translation!");
                return;
            }

            nextStateModel = stateModel;
            nextState = state;

            currentTranslation = translation;

            translation.PreviousState = CurrentState;
            translation.NextState = state;

            translation.OnTranslationBegin(translationModel);
            translation.enabled = true;
            CurrentState.OnTranslationFromStateBegin();
            state.OnTranslationToStateBegin();

        }
        public void EnableStateWithTranslation(Type stateType, Type translationType, object stateModel,
            object translationModel)
        {
            var s = GetState(stateType);
            var t = GetTranslation(translationType);


            if (s == null || t == null)
            {
                Logs.Instance.ProcessMessage("State or translation not found");
                return;
            }

            EnableStateWithTranslation(s, t, stateModel, translationModel);

        }
        public void EnableStateWithTranslation<TState, TTranslation>(object stateModel, object translationModel)
        {

            EnableStateWithTranslation(typeof(TState), typeof(TTranslation), stateModel, translationModel);
        }


        public  void RegisterTranslationComplete(GameStateTranslation translator)
        {
            if (translator == currentTranslation)
            {
                translator.enabled = false;
                currentTranslation = null;
                EnableState(nextState, nextStateModel);
                nextStateModel = null;

            }
            else
            {
                Debug.LogError("RegisterTranslationComplete(): unexpected translator passed");
            }
        }


        public  T GetState<T>() where T : GameStateBase
        {

            return States.FirstOrDefault(controller => controller.GetType() == typeof (T)) as T;
        }
        public  GameStateBase GetState(Type ct)
        {
            return States.FirstOrDefault(controller => controller.GetType() == ct);
        }

        public T GetTranslation<T>() where T : GameStateTranslation
        {
            return Translations.FirstOrDefault(t => t.GetType() == typeof(T)) as T;
        }
        public GameStateTranslation GetTranslation(Type ct)
        {
            return Translations.FirstOrDefault(controller => controller.GetType() == ct);
        }

    }

    public class EntryStateAttribute : Attribute
    {
        
    }
}
