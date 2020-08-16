// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.InputManagement
{
    /// <summary>structure containing input information collected during the last update</summary>
    public readonly struct ManageableInput
    {
        private readonly List<InputInfo> info;

        public ManageableInput(List<InputInfo> info)
        {
            this.info = info;
        }

        /// <summary>Returns whether given key is pressed</summary>
        public bool IsDown(KeyCode keyCode)
        {
            for (int i = 0; i < info.Count; i++)
            {
                if (info[i].code == keyCode && info[i].IsDown)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Returns whether given key is held down</summary>
        public bool IsHolding(KeyCode keyCode)
        {
            for (int i = 0; i < info.Count; i++)
            {
                if (info[i].code == keyCode && info[i].IsHolding)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Returns whether given key is released</summary>
        public bool IsUp(KeyCode keyCode)
        {
            for (int i = 0; i < info.Count; i++)
            {
                if (info[i].code == keyCode && info[i].IsUp)
                {
                    return true;
                }
            }
            return false;
        }
    }
}