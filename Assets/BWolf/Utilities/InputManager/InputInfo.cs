// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.InputManagement
{
    /// <summary>structure containg information on a key input done by the user</summary>
    public readonly struct InputInfo
    {
        public readonly KeyCode code;

        public static readonly int Down = 1 << 0; //0001
        public static readonly int Holding = 1 << 1; //0010
        public static readonly int Up = 1 << 2; //0100

        private readonly int arg;

        public InputInfo(KeyCode code, int arg)
        {
            this.code = code;
            this.arg = arg;
        }

        /// <summary>is the key pressed</summary>
        public bool IsDown
        {
            get { return IsPartOfArgs(Down); }
        }

        /// <summary>is the key held down</summary>
        public bool IsHolding
        {
            get { return IsPartOfArgs(Holding); }
        }

        /// <summary>is the key released</summary>
        public bool IsUp
        {
            get { return IsPartOfArgs(Up); }
        }

        /// <summary>Returns whether given integer value is part of the arg bit mask</summary>
        private bool IsPartOfArgs(int value)
        {
            return (arg & value) == value;
        }
    }
}