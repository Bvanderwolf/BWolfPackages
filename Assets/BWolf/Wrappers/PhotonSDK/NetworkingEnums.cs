namespace BWolf.Wrappers.PhotonSDK
{
    /// <summary>Defines callbacks with either no arguments or a message string </summary>
    public enum SimpleCallbackEvent
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
}