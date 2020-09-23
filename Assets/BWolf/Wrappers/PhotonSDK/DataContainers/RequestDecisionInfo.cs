// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

namespace BWolf.Wrappers.PhotonSDK.DataContainers
{
    /// <summary>structure for storing information on a decision been made by the host</summary>
    public struct RequestDecisionInfo
    {
        public int Id;
        public object DecisionContent;

        public RequestDecisionInfo(int id, object decisionContent)
        {
            Id = id;
            DecisionContent = decisionContent;
        }

        /// <summary>operation of converting request decision info to an array of objects</summary>
        public static explicit operator object[] (RequestDecisionInfo i) => new object[] { i.Id, i.DecisionContent };

        /// <summary>operation of converting an array of objects to a request decision info structure</summary>
        public static implicit operator RequestDecisionInfo(object[] objects)
        {
            RequestDecisionInfo info;
            info.Id = (int)objects[0];
            info.DecisionContent = (int)objects[1];
            return info;
        }
    }
}