﻿using System;

namespace BWolf.Wrappers.PhotonSDK.DataContainers
{
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