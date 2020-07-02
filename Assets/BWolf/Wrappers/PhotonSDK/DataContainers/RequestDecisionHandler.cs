using BWolf.Wrappers.PhotonSDK.DataContainers;

namespace BWolf.Wrappers.PhotonSDK
{
    /// <summary>delegate to decide on what decision to make and what content to send back to requesting players</summary>
    public delegate RequestDecisionInfo RequestDecisiontHandler(RunningRequest runningRequest);
}