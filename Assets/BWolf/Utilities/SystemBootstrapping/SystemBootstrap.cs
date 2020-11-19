using BWolf.Examples.SystemBootstrapping;
using UnityEngine;

namespace BWolf.Utilities.SystemBootstrapping
{
    public static class SystemBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void CreateSystems()
        {
            SystemLocator.Awake();

            SystemLocator.Instance.Register<MusicSystem>();
            SystemLocator.Instance.Register<SceneSwitchSystem>();
        }
    }
}