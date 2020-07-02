using System;

namespace BWolf.Wrappers.PhotonSDK.DataContainers
{
    /// <summary>class representation of a game event to be used by the multiplayer event handler</summary>
    public class GameEvent
    {
        public readonly string Name;
        public readonly byte EventCode;
        public bool IsRequestDecision;
        public Action<object> Callback;

        public const byte EventCodeBase = 5;

        public GameEvent(string name, byte eventCode, bool isRequestDecision = false)
        {
            Name = name;
            EventCode = eventCode;
            IsRequestDecision = isRequestDecision;
            Callback = null;
        }

        /// <summary>Add listener to callback</summary>
        public void AddListener(Action<object> action)
        {
            Callback += action;
        }

        /// <summary>Remove listener from callback</summary>
        public void RemoveListener(Action<object> action)
        {
            Callback -= action;
        }
    }
}