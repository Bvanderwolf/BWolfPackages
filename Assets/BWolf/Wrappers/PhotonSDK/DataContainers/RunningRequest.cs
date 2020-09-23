// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK.DataContainers
{
    /// <summary>class for storing information on request in progress </summary>
    public class RunningRequest
    {
        public readonly GameRequest request;
        public readonly int Id;
        public readonly Dictionary<int, string> timeStamps;

        public RunningRequest(GameRequest request, int id, int actorNumber, string timeStamp)
        {
            this.request = request;
            this.Id = id;
            this.timeStamps = new Dictionary<int, string> { { actorNumber, timeStamp } };
        }
    }
}