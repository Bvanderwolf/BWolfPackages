namespace BWolf.Wrappers.PhotonSDK.DataContainers
{
    public struct RequestDecisionInfo
    {
        public int Id;
        public object DecisionContent;

        public RequestDecisionInfo(int id, int decisionContent)
        {
            Id = id;
            DecisionContent = decisionContent;
        }

        public static explicit operator object[] (RequestDecisionInfo i) => new object[] { i.Id, i.DecisionContent };

        public static explicit operator RequestDecisionInfo(object[] objects)
        {
            RequestDecisionInfo info;
            info.Id = (int)objects[0];
            info.DecisionContent = (int)objects[1];
            return info;
        }
    }
}