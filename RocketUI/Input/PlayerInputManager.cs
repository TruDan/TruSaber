﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using RocketUI.Input.Listeners;

namespace RocketUI.Input
{
    public class InputListenerAdded
    {
        public IInputListener InputListener { get; }

        public InputListenerAdded(IInputListener inputListener)
        {
            InputListener = inputListener;
        }
    }
    
    public class PlayerInputManager
    {

        public PlayerIndex PlayerIndex { get; }
        public InputType InputType { get; private set; }

        private List<IInputListener> InputListeners { get; } = new List<IInputListener>();

        public List<InputActionBinding> Bindings { get; } = new List<InputActionBinding>();

        public EventHandler<InputListenerAdded> InputListenerAdded;
        
        public PlayerInputManager(PlayerIndex playerIndex, InputType inputType = InputType.GamePad)
        {
            PlayerIndex = playerIndex;
            InputType = inputType;
            
            AddListener(new GamePadInputListener(playerIndex));
        }

        public void AddListener(IInputListener listener)
        {
            InputListeners.Add(listener);
            
            InputListenerAdded?.Invoke(this, new InputListenerAdded(listener));
        }

        public bool TryGetListener<TType>(out TType value) where TType : IInputListener
        {
            value = default;

            var first = InputListeners.FirstOrDefault(x => typeof(TType) == x.GetType());
            if (first != default)
            {
                value = (TType) first;
                return true;
            }

            return false;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var inputListener in InputListeners.ToArray())
            {
                inputListener.Update(gameTime);
            }
        }

        public bool IsDown(InputCommand command)
        {
            return InputListeners.Any(l => l.IsDown(command));
        }

        public bool IsUp(InputCommand command)
        {
            return InputListeners.Any(l => l.IsUp(command));
        }

        public bool IsBeginPress(InputCommand command)
        {
            return InputListeners.Any(l => l.IsBeginPress(command));
        }

        public bool IsPressed(InputCommand command)
        {
            return InputListeners.Any(l => l.IsPressed(command));
        }


    }
}