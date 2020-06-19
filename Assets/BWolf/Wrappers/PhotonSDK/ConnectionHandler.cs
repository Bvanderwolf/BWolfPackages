using Photon.Pun;

namespace BWolf.Wrappers.PhotonSDK
{
    public class ConnectionHandler
    {
        /// <summary>Starts the connection witbh photon using the default settings</summary>
        public void StartDefaultConnection()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>Tries starting offline mode ouptutting any problems into the given log string</summary>
        public bool StartOffline(ref string log)
        {
            if (PhotonNetwork.OfflineMode)
            {
                log += "OfflineMode is already true";
                return false;
            }
            else if (PhotonNetwork.IsConnected)
            {
                log += "You can't start offline mode when you are connected";
                return false;
            }
            else
            {
                PhotonNetwork.OfflineMode = true;
                return true;
            }
        }

        /// <summary>Tries stopping offline mode ouptutting any problems into the given log string</summary>
        public bool StopOffline(ref string log)
        {
            if (!PhotonNetwork.OfflineMode)
            {
                log += "OfflineMode is already false";
                return false;
            }
            else
            {
                PhotonNetwork.OfflineMode = false;
                return true;
            }
        }
    }
}