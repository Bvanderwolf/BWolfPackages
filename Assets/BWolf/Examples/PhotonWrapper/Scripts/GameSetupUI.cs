using BWolf.Wrappers.PhotonSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class GameSetupUI : MonoBehaviour
    {
        [SerializeField]
        private Button btnStart = null;

        [SerializeField]
        private Text txtPlayerOne = null;

        [SerializeField]
        private Text txtPlayerTwo = null;

        private void Awake()
        {
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            NetworkingService.AddCallbackListener(InRoomCallbackEvent.ClientJoined, OnRoomUpdate);
            NetworkingService.AddCallbackListener(InRoomCallbackEvent.ClientLeft, OnRoomUpdate);
            NetworkingService.AddCallbackListener(InRoomCallbackEvent.HostChanged, OnRoomUpdate);
        }

        private void OnDestroy()
        {
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            NetworkingService.RemoveCallbackListener(InRoomCallbackEvent.ClientJoined, OnRoomUpdate);
            NetworkingService.RemoveCallbackListener(InRoomCallbackEvent.ClientLeft, OnRoomUpdate);
            NetworkingService.RemoveCallbackListener(InRoomCallbackEvent.HostChanged, OnRoomUpdate);
        }

        private void OnJoinedRoom(string message)
        {
            UpdateUIElements();
        }

        private void OnRoomUpdate(Client client)
        {
            UpdateUIElements();
        }

        private void UpdateUIElements()
        {
            //since there can only be 2 players in a room this foreach loop is feasable
            Dictionary<int, Client> clients = NetworkingService.ClientsInRoom;
            foreach (var client in clients)
            {
                if (client.Key == NetworkingService.ActorNumberOfHost)
                {
                    txtPlayerOne.text = client.Value.Nickname;
                }
                else
                {
                    txtPlayerTwo.text = client.Value.Nickname;
                }
            }

            //set start butotn interactable state based on whether room is filled
            btnStart.interactable = clients.Count == RoomItemsUI.DemoGameMaxPlayers;
        }
    }
}