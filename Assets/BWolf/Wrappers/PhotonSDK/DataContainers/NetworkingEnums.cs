﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

namespace BWolf.Wrappers.PhotonSDK.DataContainers
{
    /// <summary>Defines callbacks with either no arguments or a message string used for matchmaking</summary>
    public enum MatchmakingCallbackEvent
    {
        Connected,
        ConnectedToMaster,
        Disconnected,
        JoinedLobby,
        LeftLobby,
        CreatedRoom,
        CreateRoomFailed,
        JoinedRoom,
        JoinRoomFailed,
        JoinRandomRoomFailed,
        LeftRoom
    }

    /// <summary>Defines callbacks gained inside a room returning a client as argument</summary>
    public enum InRoomCallbackEvent
    {
        ClientJoined,
        ClientLeft,
        HostChanged
    }

    /// <summary>The group or person that will receive a networking event</summary>
    public enum EventReceivers
    {
        /// <summary>All other clients, so All excluding myself</summary>
        Other = 0,

        /// <summary>All clients including myself</summary>
        All = 1,

        /// <summary>The client hosting this room</summary>
        Host = 2
    }

    /// <summary>Defines internal events only to be used by the wrapper classes</summary>
    public enum InternalEvent : byte
    {
        StaticObjectSpawn = 1, //starts at one because zero is used as a wildcard for clearing the event cache
        StaticObjectDestroy,
        SceneObjectSpawn,
        RequestDecisionFailed
    }
}