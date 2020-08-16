// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using BWolf.Behaviours;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.InputManagement
{
    /// <summary>Singleton class for managing input and updating managable input targets</summary>
    public class InputManager : SingletonBehaviour<InputManager>
    {
        private List<Func<bool>> conditions = new List<Func<bool>>();
        private List<IInputManageable> manageables = new List<IInputManageable>();
        private List<KeyCode> keys = new List<KeyCode>();

        private void Update()
        {
            foreach (Func<bool> condition in conditions)
            {
                if (condition())
                {
                    //dont search for input if any of the conditions is true
                    return;
                }
            }

            ManageableInput input;
            if (TryGetManagableInput(out input))
            {
                //if there is input update manageable input targets
                foreach (IInputManageable manageable in manageables)
                {
                    manageable.ManageInput(input);
                }
            }
        }

        /// <summary>Adds object to manageable callback targets list</summary>
        public void AddManageable(IInputManageable managable)
        {
            manageables.Add(managable);
            AddKeys(managable.ManageableKeys);
        }

        /// <summary>Removes object from manageable callback targets list</summary>
        public void RemoveManageable(IInputManageable manageable)
        {
            manageables.Remove(manageable);
            RefreshKeys();
        }

        /// <summary>Adds condition to list of conditions to check before retreiving input</summary>
        public void AddCondition(Func<bool> condition)
        {
            conditions.Add(condition);
        }

        /// <summary>Removes condition of list of conditions to check before retreiving input</summary>
        public void RemoveCondition(Func<bool> condition)
        {
            conditions.Remove(condition);
        }

        /// <summary>Adds keys to keys to check during update frames</summary>
        private void AddKeys(IList<KeyCode> keyCodes)
        {
            for (int i = 0; i < keyCodes.Count; i++)
            {
                KeyCode key = keyCodes[i];
                bool isIn = false;
                for (int j = 0; j < keys.Count; j++)
                {
                    if (keys[j] == key)
                    {
                        isIn = true;
                        break;
                    }
                }

                if (!isIn)
                {
                    keys.Add(key);
                }
            }
        }

        /// <summary>Refreshes keys list. Call after having removed a manageable callback target</summary>
        private void RefreshKeys()
        {
            keys.Clear();
            foreach (IInputManageable manageable in manageables)
            {
                AddKeys(manageable.ManageableKeys);
            }
        }

        /// <summary>Tries outputting managable input information</summary>
        private bool TryGetManagableInput(out ManageableInput input)
        {
            List<InputInfo> info = new List<InputInfo>();
            for (int i = 0; i < keys.Count; i++)
            {
                KeyCode code = keys[i];
                int arg;
                if (GetHolding(code, out arg) | GetDown(code, ref arg) || GetUp(code, out arg))
                {
                    //if a key is held down and/or pressed, or the key is released. Add information to manageable input
                    info.Add(new InputInfo(code, arg));
                }
            }
            input = new ManageableInput(info);
            return info.Count != 0;
        }

        /// <summary>outputs whether given keycode is held down by the user</summary>
        private bool GetHolding(KeyCode code, out int arg)
        {
            arg = Input.GetKey(code) ? InputInfo.Holding : 0;
            return arg != 0;
        }

        /// <summary>outputs whether given keycode is pressed by the user</summary>
        private bool GetDown(KeyCode code, ref int arg)
        {
            if (Input.GetKeyDown(code))
            {
                //add down bits to arg bit mask (0010 -> 0011)
                arg |= InputInfo.Down;
            }

            return arg != 0;
        }

        /// <summary>outputs whether given keycode is released by the user</summary>
        private bool GetUp(KeyCode code, out int arg)
        {
            arg = Input.GetKeyUp(code) ? InputInfo.Up : 0;
            return arg != 0;
        }
    }
}