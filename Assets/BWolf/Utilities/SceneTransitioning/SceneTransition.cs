using System.Collections;
using UnityEngine;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>Reference to a scene transition being worked on by the SceneTransitionSystem</summary>
    public class SceneTransition
    {
        private OnProgressUpdate _progressUpdate;
        private bool _canShareUpdate;

        public IEnumerator OutroEnumerator { get; private set; }
        public IEnumerator IntroEnumerator { get; private set; }
        public float Progress { get; private set; }

        /// <summary>Adds an enumerator to be used as co routine before unloading the current active scene</summary>
        public SceneTransition AddOutroRoutine(IEnumerator outroEnumerator)
        {
            OutroEnumerator = outroEnumerator;

            return this;
        }

        /// <summary>Adds an enumerator to be used as co routine after loading the given scene to transition towards</summary>
        public SceneTransition AddIntroRoutine(IEnumerator introEnumerator)
        {
            IntroEnumerator = introEnumerator;

            return this;
        }

        /// <summary>Adds a listener to this transition to be called when progress has been updated</summary>
        public SceneTransition OnProgressUpdated(OnProgressUpdate progressUpdate)
        {
            _progressUpdate += progressUpdate;
            _canShareUpdate = progressUpdate != null;

            return this;
        }

        /// <summary>Updates the progress of this scene transition calling OnprogressUpdate if it has listeners</summary>
        public void UpdateProgress(float value)
        {
            Progress = Mathf.Clamp01(value);

            if (_canShareUpdate)
            {
                _progressUpdate(Progress);
            }
        }
    }
}