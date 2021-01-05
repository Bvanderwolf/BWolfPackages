// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    [CreateAssetMenu(fileName = "NewAudioEmitterFactory", menuName = "Factories/AudioEmitter Factory")]
    public class AudioEmitterFactorySO : FactorySO<AudioEmitter>
    {
        [SerializeField]
        private AudioEmitter prefab = null;

        public override AudioEmitter Create()
        {
            return Instantiate(prefab);
        }
    }
}