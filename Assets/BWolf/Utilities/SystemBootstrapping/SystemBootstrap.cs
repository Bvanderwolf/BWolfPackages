// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Examples.SystemBootstrapping;
using UnityEngine;

namespace BWolf.Utilities.SystemBootstrapping
{
    /// <summary>The static bootstrap class from which the system locator is initiated</summary>
    public static class SystemBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void CreateSystems()
        {
            //initiate system locator
            SystemLocator.Awake();

            //register systems
            SystemLocator.Instance.Register<MusicSystem>();
            SystemLocator.Instance.Register<SceneSwitchSystem>();
        }
    }
}