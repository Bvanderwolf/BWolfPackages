﻿using Photon.Realtime;
using System;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class CallbackHandler : IConnectionCallbacks, ILobbyCallbacks
    {
        private Dictionary<SimpleCallbackEvent, Action<string>> simpleCallbackEvents = new Dictionary<SimpleCallbackEvent, Action<string>>();
        private Dictionary<SimpleCallbackEvent, Action> singleSimpleCallbackEvents = new Dictionary<SimpleCallbackEvent, Action>();

        private event Action<List<LobbyData>> lobbyStatisticsUpdate;

        private event Action<List<RoomData>> roomListUpdate;

        ~CallbackHandler()
        {
            simpleCallbackEvents.Clear();
            singleSimpleCallbackEvents.Clear();
        }

        /// <summary>Adds single callback event to singlecallback events dictionary</summary>
        public void AddSingleCallback(SimpleCallbackEvent callbackEvent, Action callback)
        {
            if (singleSimpleCallbackEvents.ContainsKey(callbackEvent))
            {
                singleSimpleCallbackEvents[callbackEvent] += callback;
            }
            else
            {
                singleSimpleCallbackEvents.Add(callbackEvent, null);
                singleSimpleCallbackEvents[callbackEvent] += callback;
            }
        }

        /// <summary>Adds callback event to callback events dictionary</summary>
        public void AddListener(SimpleCallbackEvent callbackEvent, Action<string> callback)
        {
            if (simpleCallbackEvents.ContainsKey(callbackEvent))
            {
                simpleCallbackEvents[callbackEvent] += callback;
            }
            else
            {
                simpleCallbackEvents.Add(callbackEvent, null);
                simpleCallbackEvents[callbackEvent] += callback;
            }
        }

        /// <summary>Adds lobby info statistics update  callback event to lobby statistics update event invocation list</summary>
        public void AddListener(Action<List<LobbyData>> action)
        {
            lobbyStatisticsUpdate += action;
        }

        /// <summary>Adds room list update  callback event to room list update event invocation list</summary>
        public void AddListener(Action<List<RoomData>> action)
        {
            roomListUpdate += action;
        }

        /// <summary>removes callback event from callback events dictionary</summary>
        public void RemoveListener(SimpleCallbackEvent callbackEvent, Action<string> callback)
        {
            if (simpleCallbackEvents.ContainsKey(callbackEvent))
            {
                simpleCallbackEvents[callbackEvent] -= callback;
            }
        }

        /// <summary>Removes lobby statistics update callback event from invocation list</summary>
        public void RemoveListener(Action<List<LobbyData>> action)
        {
            lobbyStatisticsUpdate -= action;
        }

        /// <summary>Removes room list update callback event from invocation list</summary>
        public void RemoveListener(Action<List<RoomData>> action)
        {
            roomListUpdate -= action;
        }

        /// <summary>Called when connected initialily with the server it fires events if there are subscribers</summary>
        public void OnConnected()
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.Connected))
            {
                simpleCallbackEvents[SimpleCallbackEvent.Connected](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.Connected))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.Connected]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.Connected);
            }
        }

        /// <summary>Called when connected with the master server it fires events if there are subscribers</summary>
        public void OnConnectedToMaster()
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.ConnectedToMaster))
            {
                simpleCallbackEvents[SimpleCallbackEvent.ConnectedToMaster](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.ConnectedToMaster))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.ConnectedToMaster]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.ConnectedToMaster);
            }
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        /// <summary>Called when disconnected from the server it fires events if there are subscribers</summary>
        public void OnDisconnected(DisconnectCause cause)
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.Disconnected))
            {
                simpleCallbackEvents[SimpleCallbackEvent.Disconnected]?.Invoke(cause.ToString());
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.Disconnected))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.Disconnected]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.Disconnected);
            }
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        /// <summary>Called when having joined a lobby it fires events if there are subscribers</summary>
        public void OnJoinedLobby()
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.JoinedLobby))
            {
                simpleCallbackEvents[SimpleCallbackEvent.JoinedLobby](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.JoinedLobby))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.JoinedLobby]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.JoinedLobby);
            }
        }

        /// <summary>Called when having left a lobby it fires events if there are subscribers</summary>
        public void OnLeftLobby()
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.LeftLobby))
            {
                simpleCallbackEvents[SimpleCallbackEvent.LeftLobby](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.LeftLobby))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.LeftLobby]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.LeftLobby);
            }
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            if (roomListUpdate != null)
            {
                List<RoomData> data = new List<RoomData>();
                foreach (RoomInfo info in roomList)
                {
                    string key = (string)info.CustomProperties[RoomData.PasswordPropertyKey];
                    data.Add(RoomData.Create(info.RemovedFromList, info.Name, info.PlayerCount, info.MaxPlayers, key));
                }
                roomListUpdate(data);
            }
        }

        /// <summary>Called when lobby statistics have been updated it fires an event with lobby info if there are subscribers</summary>
        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            if (lobbyStatisticsUpdate != null)
            {
                List<LobbyData> data = new List<LobbyData>();
                foreach (TypedLobbyInfo info in lobbyStatistics)
                {
                    data.Add(LobbyData.Create(info.Name, info.PlayerCount, info.RoomCount));
                }
                lobbyStatisticsUpdate(data);
            }
        }
    }
}