using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK.Handlers
{
    public class MultiplayerEventHandler : IOnEventCallback
    {
        private Dictionary<GameEvent, Action<object>> gameEvents = new Dictionary<GameEvent, Action<object>>();
        private Dictionary<InternalEvent, Action<object>> internalEvents = new Dictionary<InternalEvent, Action<object>>();

        /// <summary>Returns whether the given code equates to a PunEvent</summary>
        private bool IsPunEvent(byte code) => code >= 200;

        /// <summary>Returns whether a code equates to a GameEvent</summary>
        private bool IsGameEvent(byte code) => Enum.IsDefined(typeof(GameEvent), code);

        /// <summary>Returns whether a code equates to a NetworkingRequestType</summary>
        private bool IsInternalEvent(byte code) => Enum.IsDefined(typeof(InternalEvent), code);

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

        /// <summary>Adds listener to given game event type</summary>
        public void AddListener(GameEvent type, Action<object> callback)
        {
            if (!gameEvents.ContainsKey(type))
            {
                gameEvents.Add(type, null);
            }

            gameEvents[type] += callback;
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
        public void RemoveListener(GameEvent type, Action<object> callbackToRemove)
        {
            if (gameEvents.ContainsKey(type) && gameEvents[type] != null)
            {
                gameEvents[type] -= callbackToRemove;
            }
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
                    case byte b when IsGameEvent(b): GameEventCallback((GameEvent)b)?.Invoke(photonEvent.CustomData); break;
                    case byte b when IsInternalEvent(b): InternalEventCallback((InternalEvent)b)?.Invoke(photonEvent.CustomData); break;
                }
            }
        }

        /// <summary>Returns delegate of given internal event type</summary>
        private Action<object> InternalEventCallback(InternalEvent type)
        {
            return internalEvents.ContainsKey(type) ? internalEvents[type] : null;
        }

        /// <summary>Returns delegate of given game event type</summary>
        private Action<object> GameEventCallback(GameEvent type)
        {
            return gameEvents.ContainsKey(type) ? gameEvents[type] : null;
        }
    }
}