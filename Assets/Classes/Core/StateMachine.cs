using System;
using System.Collections.Generic;

namespace Assets.Classes.Core
{
    public class StateMachine<TStateEnum>
    {


        private bool isInitialized;

        public void Initialize(RoseEntity entity, TStateEnum defaultState, TStateEnum initialState)
        {
            if(isInitialized) return;
            isInitialized = true;

            stateExecutors = new Dictionary<TStateEnum, Action>();
            stateEnteredListeners = new Dictionary<TStateEnum, Action<TStateEnum>>();
            stateLeavedListeners = new Dictionary<TStateEnum, Action<TStateEnum>>();
            stateUpdatedListeners = new Dictionary<TStateEnum, Action<TStateEnum>>();

            Entity = entity;
            DefaultState = defaultState;
            InitialState = initialState;
            LastState = DefaultState;
            CurrentState = initialState;
        }
        public void Initialize(RoseEntity entity, TStateEnum defaultState)
        {
            Initialize(entity, defaultState, defaultState);
        }


        public RoseEntity Entity { get; private set; }


        public TStateEnum InitialState;
        public TStateEnum DefaultState { get; private set; }
        public TStateEnum CurrentState { get; private set; }
        public TStateEnum LastState { get; private set; }


        private Dictionary<TStateEnum, Action> stateExecutors;
        private Dictionary<TStateEnum, Action<TStateEnum>> stateEnteredListeners;
        private Dictionary<TStateEnum, Action<TStateEnum>> stateLeavedListeners;
        private Dictionary<TStateEnum, Action<TStateEnum>> stateUpdatedListeners;
        private Action<TStateEnum, TStateEnum> stateChangedListener;


        private Action<TStateEnum> GetListener(Dictionary<TStateEnum, Action<TStateEnum>> d, TStateEnum s)
        {
            Action<TStateEnum> value;
            if (d.TryGetValue(s, out value))
                return value;
            return null;
        }
        private Action GetListener(Dictionary<TStateEnum, Action> d, TStateEnum s)
        {
            Action value;
            if (d.TryGetValue(s, out value))
                return value;
            return null;
        }


        public Action<TStateEnum> GetListenerForStateEntered(TStateEnum state)
        {
            return GetListener(stateEnteredListeners, state);
        }

        public Action<TStateEnum> GetListenerForStateLeaved(TStateEnum state)
        {
            return GetListener(stateLeavedListeners, state);
        }

        public Action<TStateEnum> GetListenerForStateUpdated(TStateEnum state)
        {
            return GetListener(stateUpdatedListeners, state);
        }

        public Action GetStateExecutor(TStateEnum state)
        {
            return GetListener(stateExecutors, state);
        }

        public Action<TStateEnum, TStateEnum> GetStateChangedListener()
        {
            return stateChangedListener;
        }


        private bool containsUpdateListeners;

        public void AddStateEnteredListener(TStateEnum state, Action<TStateEnum> listener)
        {
            stateEnteredListeners.Add(state, listener);
        }

        public void AddStateLeavedListener(TStateEnum state, Action<TStateEnum> listener)
        {
            stateLeavedListeners.Add(state, listener);
        }

        public void AddStateUpdatedListener(TStateEnum state, Action<TStateEnum> listener)
        {
            containsUpdateListeners = true;
            stateUpdatedListeners.Add(state, listener);
        }

        public void SetStateChangedListener(Action<TStateEnum, TStateEnum> l)
        {
            stateChangedListener = l;
        }

        public void MapStateExecutor(TStateEnum state, Action stateExecutor)
        {
            stateExecutors.Add(state, stateExecutor);
        }


        public void ChangeState(TStateEnum newState)
        {
            LastState = CurrentState;
            CurrentState = newState;

            var ll = GetListenerForStateLeaved(LastState);
            if (ll != null) ll(LastState);

            var le = GetListenerForStateEntered(CurrentState);
            if (le != null) le(CurrentState);


            if (stateChangedListener != null) stateChangedListener(CurrentState, LastState);

        }

        public void ChangeStateWithoutListeners(TStateEnum newState)
        {
            LastState = CurrentState;
            CurrentState = newState;
        }


        public void Update()
        {
            if(!containsUpdateListeners) return;

            var lu = GetListenerForStateUpdated(CurrentState);
            if (lu != null) lu(CurrentState);
        }

    }
}
