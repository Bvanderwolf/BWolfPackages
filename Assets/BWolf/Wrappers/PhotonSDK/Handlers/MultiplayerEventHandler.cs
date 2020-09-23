// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Wrappers.PhotonSDK.DataContainers;
using BWolf.Wrappers.PhotonSDK.Serialization;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK.Handlers
{
    /// <summary>Class to handle events during the game</summary>
    public class MultiplayerEventHandler : IOnEventCallback
    {
        private Dictionary<string, GameEvent> gameEvents = new Dictionary<string, GameEvent>();
        private Dictionary<InternalEvent, Action<object>> internalEvents = new Dictionary<InternalEvent, Action<object>>();

        private Dictionary<string, GameRequest> gameRequests = new Dictionary<string, GameRequest>();
        private Dictionary<int, GameRequest> requestsInProgress = new Dictionary<int, GameRequest>();
        private List<RunningRequest> runningRequests = new List<RunningRequest>();

        private ConcurrentBag<int> nonTargetIds = new ConcurrentBag<int>();
        private const int nonTargetIdFillAmount = 10;
        private int nonTargetIdCount;

        /// <summary>Returns whether the given code equates to a PunEvent</summary>
        private bool IsPunEvent(byte code) => code >= 200;

        /// <summary>Returns whether a code equates to a GameEvent</summary>
        private bool IsGameEvent(byte code) => gameEvents.Values.Any(g => g.EventCode == code);

        /// <summary>Returns whether a code equates to a GameRequest</summary>
        private bool IsGameRequest(byte code) => gameRequests.Values.Any(g => g.EventCode == code);

        /// <summary>Returns whether a code equates to a NetworkingRequestType</summary>
        private bool IsInternalEvent(byte code) => Enum.IsDefined(typeof(InternalEvent), code);

        private SerializableTypes serialization;
        private const string requestDecisionKey = "Decision";

        public MultiplayerEventHandler(SerializableTypes serialization)
        {
            this.serialization = serialization;
        }

        /// <summary>Raises event of given type, with given content, send at given receivergroup and with given reliability</summary>
        public void RaiseGameEvent(string nameOfEvent, object content, ReceiverGroup receivers, bool sendReliable)
        {
            RaiseEvent(gameEvents[nameOfEvent].EventCode, content, receivers, sendReliable);
        }

        /// <summary>Raises event of given type, with given content, send at given receivergroup and with given reliability</summary>
        public void RaiseGameEvent(string nameOfEvent, object content, int[] targetActorNumbers, bool sendReliable)
        {
            RaiseEvent(gameEvents[nameOfEvent].EventCode, content, targetActorNumbers, sendReliable);
        }

        /// <summary>Raises event of given type, with given content, send at given receivergroup and with given reliability</summary>
        public void RaiseEvent(byte eventCode, object content, ReceiverGroup receivers, bool sendReliable)
        {
            RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = receivers };
            SendOptions sendOptions = sendReliable ? SendOptions.SendReliable : SendOptions.SendUnreliable;
            PhotonNetwork.RaiseEvent(eventCode, content, eventOptions, sendOptions);
        }

        /// <summary>Raises event of given type, with given content, send at given receivergroup and with given reliability</summary>
        public void RaiseEvent(byte eventCode, object content, int[] targetActorNumbers, bool sendReliable)
        {
            RaiseEventOptions eventOptions = new RaiseEventOptions { TargetActors = targetActorNumbers };
            SendOptions sendOptions = sendReliable ? SendOptions.SendReliable : SendOptions.SendUnreliable;
            PhotonNetwork.RaiseEvent(eventCode, content, eventOptions, sendOptions);
        }

        /// <summary>Raises a game request using given name and target view id</summary>
        public void RaiseGameRequest(string nameOfRequest, int targetViewId)
        {
            GameRequest request = gameRequests[nameOfRequest];

            if (!request.StartaHandler(targetViewId)) { return; }

            object[] content = new object[] { targetViewId, DateTime.UtcNow.TimeOfDay.ToString() };

            if (requestsInProgress.ContainsKey(targetViewId))
            {
                //if a request with the same target viewID is already in waiting, we exit;
                return;
            }

            //add request item with targets viewID as key and requst as value to invoke callback when a decision has been made by the host
            requestsInProgress.Add(targetViewId, request);
            //send event to host only
            RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
            PhotonNetwork.RaiseEvent(request.EventCode, content, eventOptions, SendOptions.SendReliable);
        }

        /// <summary>Raises game request using only given name generating a non target view id instead of using a target view id</summary>
        public void RaiseGameRequest(string nameOfRequest)
        {
            int id;
            while (!nonTargetIds.TryTake(out id))
            {
                FillNonTargetIds();
            }

            RaiseGameRequest(nameOfRequest, id);
        }

        /// <summary>registers game event using given name of event and the type of content it needs to use</summary>
        public void RegisterGameEvent(string nameOfEvent, Type contentType)
        {
            if (!contentType.IsSerializable && !serialization.SerializedTypes.Contains(contentType))
            {
                Debug.LogWarningFormat("Registering game event {0} with content of type {1} failed :: content is not serializable", nameOfEvent, contentType);
                return;
            }

            if (!gameEvents.ContainsKey(nameOfEvent))
            {
                gameEvents.Add(nameOfEvent, new GameEvent(nameOfEvent, (byte)(GameEvent.EventCodeBase + gameEvents.Count)));
            }
        }

        /// <summary>registers game request using given name, type of content, request delay, start handler and decision handler</summary>
        public void RegisterGameRequest(string nameOfRequest, Type contentType, int requestDelayMiliseconds, RequestStartHandler startHandler, RequestDecisiontHandler decisionHandler)
        {
            if (!contentType.IsSerializable && !serialization.SerializedTypes.Contains(contentType))
            {
                Debug.LogWarningFormat("Registering game event {0} with content of type {1} failed :: content is not serializable", nameOfRequest, contentType);
                return;
            }

            if (!gameRequests.ContainsKey(nameOfRequest))
            {
                gameRequests.Add(nameOfRequest, new GameRequest(nameOfRequest, (byte)(GameRequest.EventCodeBase + gameEvents.Count), requestDelayMiliseconds, startHandler, decisionHandler));

                //add game event for request decision to be send
                string nameOfDecisionEvent = nameOfRequest + requestDecisionKey;
                gameEvents.Add(nameOfDecisionEvent, new GameEvent(nameOfDecisionEvent, (byte)(GameEvent.EventCodeBase + gameEvents.Count), true));
            }
        }

        /// <summary>Adds listener to given game event type</summary>
        public void AddGameEventListener(string nameOfGameEvent, Action<object> callback)
        {
            gameEvents[nameOfGameEvent].AddListener(callback);
        }

        /// <summary>Adds listener to given game request decision type</summary>
        public void AddGameRequestListener(string nameOfGameRequest, Action<object> callback)
        {
            gameEvents[nameOfGameRequest + requestDecisionKey].AddListener(callback);
        }

        /// <summary>Adds listener to given internal event type</summary>
        public void AddListener(InternalEvent type, Action<object> callback)
        {
            if (!internalEvents.ContainsKey(type))
            {
                internalEvents.Add(type, null);
            }

            internalEvents[type] += callback;
        }

        /// <summary>Removes listener from given game event type</summary>
        public void RemoveGameEventListener(string nameOfGameEvent, Action<object> callback)
        {
            gameEvents[nameOfGameEvent].RemoveListener(callback);
        }

        /// <summary>Removes listener from given game request decision type</summary>
        public void RemoveGameRequestListener(string nameOfGameRequest, Action<object> callback)
        {
            gameEvents[nameOfGameRequest + requestDecisionKey].RemoveListener(callback);
        }

        /// <summary>Removes listener from given internal event type</summary>
        public void RemoveListener(InternalEvent type, Action<object> callbackToRemove)
        {
            if (internalEvents.ContainsKey(type) && internalEvents[type] != null)
            {
                internalEvents[type] -= callbackToRemove;
            }
        }

        /// <summary>Handles event callbacks by firing an event based on the type of photon event that was fired</summary>
        public void OnEvent(EventData eventData)
        {
            byte code = eventData.Code;
            if (!IsPunEvent(code))
            {
                switch (code)
                {
                    case byte b when IsInternalEvent(b): InternalEventCallback((InternalEvent)b)?.Invoke(eventData.CustomData); break;
                    case byte b when IsGameEvent(b):
                        {
                            object data = eventData.CustomData;
                            if (GetGameEventWithCode(b).IsRequestDecision)
                            {
                                RequestDecisionInfo info = (object[])data;
                                data = info.DecisionContent;
                                requestsInProgress.Remove(info.Id);
                                if (info.Id < 0)
                                {
                                    nonTargetIds.Add(info.Id);
                                }
                            }

                            GameEventCallback(b)?.Invoke(data);
                            break;
                        }

                    case byte b when IsGameRequest(b):
                        {
                            if (!PhotonNetwork.IsMasterClient)
                            {
                                Debug.LogWarning("Did request to a client other than host :: This is not intended behaviour!");
                                return;
                            }

                            HandleGameRequest(eventData);
                            break;
                        }
                }
            }
        }

        /// <summary>handles game request using given event data, checking whether it is valid and run it if possible</summary>
        private void HandleGameRequest(EventData eventData)
        {
            GameRequest request = gameRequests.Values.FirstOrDefault(g => g.EventCode == eventData.Code);
            object[] content = (object[])eventData.CustomData;
            string timeStamp = (string)content[1];
            int targetViewId = (int)content[0];

            if (targetViewId > 0 && !ResourceHandler.Find(targetViewId).activeInHierarchy)
            {
                //make sure when request arives, the object the request is about is an active item in the scene
                string errorMessage = "Failed handling request as host for " + request.Name + " :: gameobject was inactive.";
                object[] errorContent = new object[] { errorMessage, targetViewId };

                Debug.LogWarning(errorMessage);
                RaiseEventOptions eventOptions = new RaiseEventOptions { TargetActors = new int[] { eventData.Sender } };
                PhotonNetwork.RaiseEvent((byte)InternalEvent.RequestDecisionFailed, errorMessage, eventOptions, SendOptions.SendReliable);
                return;
            }

            RunningRequest runningRequest = runningRequests.FirstOrDefault(r => r.Id == targetViewId);
            if (runningRequest == null)
            {
                //if no request of this type and target is already in progress, add a new one to the list and start handling it
                runningRequest = new RunningRequest(request, targetViewId, eventData.Sender, timeStamp);
                runningRequests.Add(runningRequest);
                RunGameRequest(runningRequest, request.RequestDelay);
            }
            else
            {
                //if a request with the same target and type is already in progress we just add this actors number and timestamp to the request's dictionary
                runningRequest.timeStamps.Add(eventData.Sender, timeStamp);
            }
        }

        /// <summary>Runs game request by waiting for given delay to send request decision info afterwards</summary>
        private async void RunGameRequest(RunningRequest requestToRun, int delay)
        {
            await Task.Delay(delay);

            GameEvent gameEvent = gameEvents[requestToRun.request.Name + requestDecisionKey];
            RequestDecisionInfo info = requestToRun.request.DecisionHandler(requestToRun);

            RaiseEvent(gameEvent.EventCode, (object[])info, requestToRun.timeStamps.Keys.ToArray(), true);
            runningRequests.Remove(requestToRun);
        }

        /// <summary>Returns delegate of given internal event type</summary>
        private Action<object> InternalEventCallback(InternalEvent type)
        {
            return internalEvents.ContainsKey(type) ? internalEvents[type] : null;
        }

        /// <summary>Returns delegate of game event with byte code</summary>
        private Action<object> GameEventCallback(byte code)
        {
            return gameEvents.Values.FirstOrDefault(g => g.EventCode == code)?.Callback;
        }

        /// <summary>Fills non target ids bag with negative values starting from non target id count and upwards</summary>
        private void FillNonTargetIds()
        {
            for (int count = 1; count < nonTargetIdFillAmount + 1; count++)
            {
                nonTargetIds.Add(-(nonTargetIdCount + count));
            }

            nonTargetIdCount += nonTargetIdFillAmount;
        }

        /// <summary>Returns a game event based on given byte code. Returns null if no game event is found</summary>
        private GameEvent GetGameEventWithCode(byte code)
        {
            foreach (GameEvent gameEvent in gameEvents.Values)
            {
                if (gameEvent.EventCode == code)
                {
                    return gameEvent;
                }
            }
            return null;
        }
    }
}