// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.InputManagement
{
    /// <summary>interface to be implemented for getting input information from the InputManager</summary>
    public interface IInputManageable
    {
        /// <summary>A collection of keycodes in the form of e.g. a list or array to be used by the input manager</summary>
        IList<KeyCode> ManageableKeys { get; }

        /// <summary>method to act on input retreived by the input manager</summary>
        void ManageInput(ManageableInput input);
    }
}