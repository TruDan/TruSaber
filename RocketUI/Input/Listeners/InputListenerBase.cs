﻿using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RocketUI.Input.Listeners
{
    public abstract class InputListenerBase<TState, TButtons> : IInputListener, IEnumerable<KeyValuePair<InputCommand, TButtons>>
    {
        public PlayerIndex PlayerIndex { get; }

        private readonly IDictionary<InputCommand, TButtons> _buttonMap = new Dictionary<InputCommand, TButtons>();

        protected TState PreviousState, CurrentState;

        protected abstract TState GetCurrentState();

        protected abstract bool IsButtonDown(TState state, TButtons buttons);
        protected abstract bool IsButtonUp(TState state, TButtons buttons);

        protected InputListenerBase(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }

        public void Update(GameTime gameTime)
        {
            PreviousState = CurrentState;
            CurrentState = GetCurrentState();

            OnUpdate(gameTime);
        }

        protected virtual void OnUpdate(GameTime gameTime)
        {

        }

        public void RegisterMap(InputCommand command, TButtons buttons)
        {
            if (_buttonMap.ContainsKey(command))
            {
                _buttonMap[command] = buttons;
            }
            else
            {
                _buttonMap.Add(command, buttons);
            }
        }

        public void RemoveMap(InputCommand command)
        {
            if (_buttonMap.ContainsKey(command))
                _buttonMap.Remove(command);
        }
        
        public bool IsDown(InputCommand command)
        {
            return (TryGetButtons(command, out var buttons) && IsButtonDown(CurrentState, buttons));
        }

        public bool IsUp(InputCommand command)
        {
            return (TryGetButtons(command, out var buttons) && IsButtonUp(CurrentState, buttons));
        }
        
        public bool IsBeginPress(InputCommand command)
        {
            return (TryGetButtons(command, out var buttons) && IsButtonDown(CurrentState, buttons) && IsButtonUp(PreviousState, buttons));
        }

        public bool IsPressed(InputCommand command)
        {
            return (TryGetButtons(command, out var buttons) && IsButtonUp(CurrentState, buttons) && IsButtonDown(PreviousState, buttons));
        }
        
        private bool TryGetButtons(InputCommand command, out TButtons buttons)
        {
            return _buttonMap.TryGetValue(command, out buttons);
        }

        public IEnumerator<KeyValuePair<InputCommand, TButtons>> GetEnumerator()
        {
            foreach (var kv in _buttonMap)
                yield return kv;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
