namespace BWolf.Wrappers.PhotonSDK.DataContainers
{
    /// <summary>class representation of a game request to be used by the multiplayer event handler</summary>
    public class GameRequest
    {
        public readonly string Name;
        public readonly byte EventCode;
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
        }
    }
}