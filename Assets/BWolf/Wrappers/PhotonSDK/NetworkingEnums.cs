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
}