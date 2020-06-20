using System;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class CallbackHandler : IConnectionCallbacks, ILobbyCallbacks
    {
        private Dictionary<CallbackEvent, Action<string>> callbackEvents = new Dictionary<CallbackEvent, Action<string>>();
        private Dictionary<CallbackEvent, Action> singleCallbackEvents = new Dictionary<CallbackEvent, Action>();

        ~CallbackHandler()
        {
            callbackEvents.Clear();
            singleCallbackEvents.Clear();
        }

        public void AddSingleCallback(CallbackEvent callbackEvent, Action callback)
        {
            if (singleCallbackEvents.ContainsKey(callbackEvent))
            {
                singleCallbackEvents[callbackEvent] += callback;
            }
            else
            {
                singleCallbackEvents.Add(callbackEvent, null);
                singleCallbackEvents[callbackEvent] += callback;
            }
        }

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
                callbackEvents[CallbackEvent.Connected](null);
            }
            if (singleCallbackEvents.ContainsKey(CallbackEvent.Connected))
            {
                singleCallbackEvents[CallbackEvent.Connected]();
                singleCallbackEvents.Remove(CallbackEvent.Connected);
            }
        }

        public void OnConnectedToMaster()
        {
            if (callbackEvents.ContainsKey(CallbackEvent.ConnectedToMaster))
            {
                callbackEvents[CallbackEvent.ConnectedToMaster](null);
            }
            if (singleCallbackEvents.ContainsKey(CallbackEvent.ConnectedToMaster))
            {
                singleCallbackEvents[CallbackEvent.ConnectedToMaster]();
                singleCallbackEvents.Remove(CallbackEvent.ConnectedToMaster);
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
                callbackEvents[CallbackEvent.Disconnected](cause.ToString());
            }
            if (singleCallbackEvents.ContainsKey(CallbackEvent.Disconnected))
            {
                singleCallbackEvents[CallbackEvent.Disconnected]();
                singleCallbackEvents.Remove(CallbackEvent.Disconnected);
            }
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnJoinedLobby()
        {
            if (callbackEvents.ContainsKey(CallbackEvent.JoinedLobby))
            {
                callbackEvents[CallbackEvent.JoinedLobby](null);
            }
            if (singleCallbackEvents.ContainsKey(CallbackEvent.JoinedLobby))
            {
                singleCallbackEvents[CallbackEvent.JoinedLobby]();
                singleCallbackEvents.Remove(CallbackEvent.JoinedLobby);
            }
        }

        public void OnLeftLobby()
        {
            if (callbackEvents.ContainsKey(CallbackEvent.LeftLobby))
            {
                callbackEvents[CallbackEvent.LeftLobby](null);
            }
            if (singleCallbackEvents.ContainsKey(CallbackEvent.LeftLobby))
            {
                singleCallbackEvents[CallbackEvent.LeftLobby]();
                singleCallbackEvents.Remove(CallbackEvent.LeftLobby);
            }
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }
    }
}