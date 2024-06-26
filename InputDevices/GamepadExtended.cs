﻿//(c) Jesper Niedermann 2010
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JumperLibrary
{
    public class GamepadExtended : InputDeviceExtended<GamePadState>
    {
        private bool m_HasDeadZoneMode = false;
        private PlayerIndex m_PlayerIndex = PlayerIndex.One;
        private GamePadDeadZone m_GamePadDeadZone;

        public GamepadExtended(PlayerIndex playerIndex) 
        {
            m_PlayerIndex = playerIndex;
        }

        public GamepadExtended(PlayerIndex playerIndex, GamePadDeadZone gamePadDeadZone) : this(playerIndex)
        {
            m_HasDeadZoneMode = true;
            m_GamePadDeadZone = gamePadDeadZone;
        }

        private static Dictionary<PlayerIndex, GamepadExtended> m_CurrentDictionary;
        private static Dictionary<string, GamepadExtended> m_CurrentDictionaryWDeadZone;

        public static GamepadExtended Current(PlayerIndex index)
        {
            if (m_CurrentDictionary == null)
            {
                m_CurrentDictionary = new Dictionary<PlayerIndex, GamepadExtended>(4);
            }

            if (!m_CurrentDictionary.ContainsKey(index))
            {
                m_CurrentDictionary.Add(index, new GamepadExtended(index));
            }
            return m_CurrentDictionary[index];
        }

        public static GamepadExtended Current(PlayerIndex index, GamePadDeadZone gamePadDeadZone)
        {
            if (m_CurrentDictionaryWDeadZone == null)
            {
                m_CurrentDictionaryWDeadZone = new Dictionary<string, GamepadExtended>(4);
            }

            string key = ComboKey(index, gamePadDeadZone);
            if (!m_CurrentDictionaryWDeadZone.ContainsKey(key))
            {
                m_CurrentDictionaryWDeadZone.Add(key, new GamepadExtended(index, gamePadDeadZone));
            }
            return m_CurrentDictionaryWDeadZone[key];
        }

        private static string ComboKey(PlayerIndex index, GamePadDeadZone gamePadDeadZone)
        {
            return string.Format("{0};{1}", index.ToString(), gamePadDeadZone.ToString());
        }

        public GamePadState GetState(GameTime currentTime)
        {
            DequeueOldStates(currentTime);
            GamePadState state;
            if (m_HasDeadZoneMode)
            {
                state = GamePad.GetState(m_PlayerIndex, m_GamePadDeadZone);
            }
            else
            {
                state = GamePad.GetState(m_PlayerIndex);
            }
            EnqueueNewState(currentTime, state);
            return state;
        }

        private bool ClickCount(Buttons checkButton, int requiredCount)
        {
            bool buttonWasDown = false;
            int count = 0;
            foreach (InputStateExtended<GamePadState> stateExt in RecordedStates)
            {
                if (buttonWasDown && stateExt.State.IsButtonUp(checkButton))
                {
                    count++;
                    if (count >= requiredCount)
                        return true;
                }
                buttonWasDown = stateExt.State.IsButtonUp(checkButton);
            }
            return false;
        }

        public bool WasSingleClick(Buttons checkButton)
        {
            return ClickCount(checkButton, 1);
        }

        public bool WasDoubleClick(Buttons checkButton)
        {
            return ClickCount(checkButton, 2);
        }
    }
}

