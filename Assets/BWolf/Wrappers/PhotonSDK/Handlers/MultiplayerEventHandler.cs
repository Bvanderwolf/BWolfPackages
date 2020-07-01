using BWolf.Wrappers.PhotonSDK.DataContainers;
using BWolf.Wrappers.PhotonSDK.Serialization;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK.Handlers
{
    internal class MultiplayerEventHandler : IOnEventCallback
    {
        private Dictionary<string, GameEvent> gameEvents = new Dictionary<string, GameEvent>();
        private Dictionary<InternalEvent, Action<object>> internalEvents = new Dictionary<InternalEvent, Action<object>>();

        /// <summary>Returns whether the given code equates to a PunEvent</summary>
        private bool IsPunEvent(byte code) => code >= 200;

        /// <summary>Returns whether a code equates to a GameEvent</summary>
        private bool IsGameEvent(byte code) => gameEvents.Values.Any(g => g.Code == code);

        /// <summary>Returns whether a code equates to a NetworkingRequestType</summary>
        private bool IsInternalEvent(byte code) => Enum.IsDefined(typeof(InternalEvent), code);

        private SerializableTypes serialization;

        public MultiplayerEventHandler(SerializableTypes serialization)
        {
            this.serialization = serialization;
        }

        /// <summary>Raises event of given type, with given content, send at given receivergroup and with given reliability</summary>
        public void RaiseGameEvent(string nameOfEvent, object content, ReceiverGroup receivers, bool sendReliable)
        {
            RaiseEvent(gameEvents[nameOfEvent].Code, content, receivers, sendReliable);
        }

        /// <summary>Raises event of given type, with given content, send at given receivergroup and with given reliability</summary>
        public void RaiseGameEvent(string nameOfEvent, object content, int[] targetActorNumbers, bool sendReliable)
        {
            RaiseEvent(gameEvents[nameOfEvent].Code, content, targetActorNumbers, sendReliable);
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

        internal void RegisterGameEvent(string nameOfEvent, Type contentType)
        {
            if (!contentType.IsSerializable && !serialization.SerializedTypes.Contains(contentType))
            {
                Debug.LogWarningFormat("Registering game event {0} with content of type {1} failed :: content is not serializable", nameOfEvent, contentType);
                return;
            }

            if (!gameEvents.ContainsKey(nameOfEvent))
            {
                gameEvents.Add(nameOfEvent, new GameEvent(nameOfEvent, (byte)gameEvents.Count));
            }
        }

        /// <summary>Adds listener to given game event type</summary>
        public void AddListener(string nameOfGameEvent, Action<object> callback)
        {
            gameEvents[nameOfGameEvent].AddListener(callback);
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
        public void RemoveListener(string nameOfGameEvent, Action<object> callback)
        {
            gameEvents[nameOfGameEvent].RemoveListener(callback);
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
        public void OnEvent(EventData photonEvent)
        {
            byte code = photonEvent.Code;
            if (!IsPunEvent(code))
            {
                switch (code)
                {
                    case byte b when IsGameEvent(b): GameEventCallback(b)?.Invoke(photonEvent.CustomData); break;
                    case byte b when IsInternalEvent(b): InternalEventCallback((InternalEvent)b)?.Invoke(photonEvent.CustomData); break;
                }
            }
        }

        /// <summary>Returns delegate of given internal event type</summary>
        private Action<object> InternalEventCallback(InternalEvent type)
        {
            return internalEvents.ContainsKey(type) ? internalEvents[type] : null;
        }

        /// <summary>Returns delegate of game event with byte code</summary>
        private Action<object> GameEventCallback(byte code)
        {
            return gameEvents.Values.FirstOrDefault(g => g.Code == code)?.Callback;
        }
    }
}