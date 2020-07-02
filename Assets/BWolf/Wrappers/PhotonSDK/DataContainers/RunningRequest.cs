using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK.DataContainers
{
    /// <summary>Nested class for storing request in progress information</summary>
    public class RunningRequest
    {
        public readonly GameRequest request;
        public readonly int targetViewId;
        public readonly Dictionary<int, string> timeStamps;

        public RunningRequest(GameRequest request, int targetViewId, int actorNumber, string timeStamp)
        {
            this.request = request;
            this.targetViewId = targetViewId;
            this.timeStamps = new Dictionary<int, string> { { actorNumber, timeStamp } };
        }
    }
}