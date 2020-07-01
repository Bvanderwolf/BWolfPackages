using System;

namespace BWolf.Wrappers.PhotonSDK.DataContainers
{
    public class GameEvent
    {
        public readonly string Name;
        public readonly byte Code;
        public Action<object> Callback;

        public GameEvent(string name, byte code)
        {
            Name = name;
            Code = code;
            Callback = null;
        }

        public void AddListener(Action<object> action)
        {
            Callback += action;
        }

        public void RemoveListener(Action<object> action)
        {
            Callback -= action;
        }
    }
}