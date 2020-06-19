using System;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class CallbackHandler : IConnectionCallbacks
    {
        private Dictionary<CallbackEvent, Action<string>> callbackEvents = new Dictionary<CallbackEvent, Action<string>>();

        public void AddListener(CallbackEvent callbackEvent, Action<string> callback)
        {
            if (callbackEvents.ContainsKey(callbackEvent))
            {
                callbackEvents[callbackEvent] += callback;
            }
            else
            {
                callbackEvents.Add(callbackEvent, null);
                callbackEvents[callbackEvent] += callback;
            }
        }

        public void RemoveListener(CallbackEvent callbackEvent, Action<string> callback)
        {
            if (callbackEvents.ContainsKey(callbackEvent))
            {
                callbackEvents[callbackEvent] -= callback;
            }
        }

        public void OnConnected()
        {
            if (callbackEvents.ContainsKey(CallbackEvent.Connected))
            {
                callbackEvents[CallbackEvent.Connected]?.Invoke(null);
            }
        }

        public void OnConnectedToMaster()
        {
            if (callbackEvents.ContainsKey(CallbackEvent.ConnectedToMaster))
            {
                callbackEvents[CallbackEvent.ConnectedToMaster]?.Invoke(null);
            }
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            if (callbackEvents.ContainsKey(CallbackEvent.Disconnected))
            {
                callbackEvents[CallbackEvent.Disconnected]?.Invoke(cause.ToString());
            }
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }
    }
}