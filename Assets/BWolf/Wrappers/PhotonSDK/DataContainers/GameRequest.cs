using System;

namespace BWolf.Wrappers.PhotonSDK.DataContainers
{
    public class GameRequest
    {
        public readonly string Name;
        public readonly byte EventCode;
        public Action<object> Callback;
        public RequestStartHandler StartaHandler;
        public RequestDecisiontHandler DecisionHandler;
        public int RequestDelay;

        public const byte EventCodeBase = 50;

        public GameRequest(string name, byte eventCode, int requestDelayMiliSeconds, RequestStartHandler startHandler, RequestDecisiontHandler decisionHandler)
        {
            Name = name;
            EventCode = eventCode;
            RequestDelay = requestDelayMiliSeconds;
            StartaHandler = startHandler;
            DecisionHandler = decisionHandler;
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