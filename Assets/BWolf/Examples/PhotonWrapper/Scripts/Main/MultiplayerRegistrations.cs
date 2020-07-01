using BWolf.Examples.PhotonWrapper.Game;
using BWolf.Wrappers.PhotonSDK;

namespace BWolf.Examples.PhotonWrapper.Main
{
    public static class MultiplayerRegistrations
    {
        private static bool registered;

        public static void Register()
        {
            if (registered) { return; }

            NetworkingService.RegisterCustomSerializable(typeof(TurnFinishedInfo), 'T', TurnFinishedInfo.Serialize, TurnFinishedInfo.Deserialize);
            NetworkingService.RegisterGameEvent(TurnManager.NameOfTurnFinishedEvent, typeof(TurnFinishedInfo));
            registered = true;
        }
    }
}