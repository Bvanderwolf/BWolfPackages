// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    [CreateAssetMenu(fileName = "NewAudioEmitterPool", menuName = "Pools/AudioEmitter Pool")]
    public class AudioEmitterPoolSO : ComponentPoolSO<AudioEmitter>
    {
        [SerializeField]
        private AudioEmitterFactorySO factory = null;

        public override IFactory<AudioEmitter> Factory
        {
            get
            {
                return factory;
            }
            set
            {
                factory = value as AudioEmitterFactorySO;
            }
        }
    }
}