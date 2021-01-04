using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    public class ThemeStarter : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private AudioConfigurationSO config = null;

        [SerializeField]
        private AudioCueSO themeCue = null;

        [Header("Channel broadcasting on")]
        [SerializeField]
        private AudioRequestChannelSO channel = null;

        private void Start()
        {
            channel.RaiseEvent(config, themeCue, Vector3.zero);
        }
    }
}